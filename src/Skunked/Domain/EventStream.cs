using System.Collections;
using System.Collections.Immutable;

namespace Skunked;

/// <summary>
/// A stream of events.
/// </summary>
public sealed class EventStream : IEnumerable<StreamEvent>
{
    private readonly object _locker = new();

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
    internal void Add(StreamEvent @event)
    {
        lock (_locker)
        {
            var lastEvent = _events.LastOrDefault();
            if (lastEvent != null && @event.Occurred <= lastEvent.Occurred)
            {
                throw new InvalidOperationException(
                    $"Concurrency problem detected. Given event occurred at {@event.Occurred:F} and before last recorded event at {lastEvent.Occurred:F} ");
            }

            _events.Add(CopyEvent(@event));
            foreach (var eventListener in _eventListeners)
            {
                eventListener.Notify(CopyEvent(@event));
            }
        }
    }

    private static StreamEvent CopyEvent(StreamEvent item) => item switch
    {
        GameStartedEvent e => e with { Players = e.Players.ToList() },
        DeckShuffledEvent e => e with { Deck = e.Deck.ToList() },
        HandsDealtEvent e => e with { Hands = e.Hands.Select(GameStateSnapshot.CopyHand).ToList() },
        CardsThrownEvent e => e with { Thrown = e.Thrown.ToList() },
        _ => item
    };

    /// <inheritdoc/>
    public IEnumerator<StreamEvent> GetEnumerator()
    {
        lock (_locker) return _events.Select(CopyEvent).ToList().GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}