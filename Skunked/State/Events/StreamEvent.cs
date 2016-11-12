using System;

namespace Skunked.State.Events
{
    //should add an Guid?
    public abstract class StreamEvent
    {
        protected StreamEvent()
        {
            Occurred = DateTimeOffset.Now;
            EventType = GetType().Name;
        }

        public Guid GameId { get; set; }
        public int Sequence { get; set; }
        public DateTimeOffset Occurred { get; set; }

        /// <summary>
        /// Exposes StreamEvent type as a field.  Used to play nicely with unstructured serializers like json.
        /// </summary>
        public string EventType { get; set; }
    }
}
