using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Skunked.Domain.Events;

namespace Skunked.Domain
{
    /// <summary>
    /// A stream of events.
    /// </summary>
    public class EventStream : IEnumerable<StreamEvent>
    {
        private static readonly object Locker = new ();

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
                var lastEvent = _events.LastOrDefault();
                if (lastEvent != null && @event.Occurred <= lastEvent.Occurred)
                {
                    throw new InvalidOperationException($"Concurrency problem detected. Given event occurred at {@event.Occurred:F} and before last recorded event at {lastEvent.Occurred:F} ");
                }

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
}
