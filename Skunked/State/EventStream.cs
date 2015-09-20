using System.Collections;
using System.Collections.Generic;
using Skunked.State.Events;

namespace Skunked.State
{
    public class EventStream : IEnumerable<StreamEvent>
    {
        private readonly List<IEventListener> _eventListeners;
        private readonly List<StreamEvent> _events;
        private static readonly object Locker = new object();

        public EventStream(List<IEventListener> eventListeners)
        {
            _eventListeners = eventListeners;
            _events = new List<StreamEvent>(100);
        }

        public void Add(StreamEvent @event)
        {
            lock (Locker)
            {
                @event.Sequence = _events.Count + 1;
                _events.Add(@event);
                foreach (var eventListener in _eventListeners)
                {
                    dynamic castEvent = eventListener;
                    eventListener.Notify(@event);
                }
            }
        }

        public IEnumerator<StreamEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
