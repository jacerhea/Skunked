using System.Collections;
using System.Collections.Immutable;

namespace Skunked;

/// <summary>
/// A stream of events.
/// </summary>
public sealed class EventStream : IEnumerable<StreamEvent>
{
    private static readonly object Locker = new();

    private readonly ImmutableList<IEventListener> _eventListeners;
    private readonly List<StreamEvent> _events;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventStream"/> class.
    /// </summary>
    /// <param name="eventListeners">Listeners for events.</param>
    public EventStream(IEnumerable<IEventListener> eventListeners)
    {
        _eventListeners = eventListeners.ToImmutableList();
        _events = new List<StreamEvent>(100);
    }

    /// <summary>
    /// Add an event to the stream.
    /// </summary>
    /// <param name="event">The event to add.</param>
    public void Add(StreamEvent @event)
    {
        lock (Locker)
        {
            ArgumentNullException.ThrowIfNull(@event);
            var lastEvent = _events.LastOrDefault();
            if (lastEvent is null)
            {
                if (@event is not GameStartedEvent || @event.Version != 1 || @event.GameId == Guid.Empty)
                    throw new InvalidOperationException("A stream must start with version 1 of a game.");
            }
            else if (@event.GameId != lastEvent.GameId || @event.Version != lastEvent.Version + 1)
                throw new InvalidOperationException("Event game ID or version does not match the stream.");

            _events.Add(@event);
            foreach (var eventListener in _eventListeners)
            {
                eventListener.Notify(@event);
            }
        }
    }

    /// <inheritdoc/>
    public IEnumerator<StreamEvent> GetEnumerator()
    {
        return _events.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}