using System.Collections;
using System.Collections.Generic;

namespace Skunked.State.Events
{
    public class EventStream : IEnumerable<Event>
    {
        private readonly List<IEventListener> _eventListeners;
        private readonly List<Event> _events;

        public EventStream(List<IEventListener> eventListeners)
        {
            _eventListeners = eventListeners;
            _events = new List<Event>(100);
        }

        public void Add(Event @event)
        {
            _events.Add(@event);
            foreach (var eventListener in _eventListeners)
            {
                eventListener.Notify(@event);
            }
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
