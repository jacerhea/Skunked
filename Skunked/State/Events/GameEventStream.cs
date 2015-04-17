using System.Collections;
using System.Collections.Generic;

namespace Skunked.State.Events
{
    public class GameEventStream : IEnumerable<Event>
    {
        private readonly List<Event> _events;

        public GameEventStream()
        {
            _events = new List<Event>(50);
        }

        public void Add(Event @event)
        {
            _events.Add(@event);
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
