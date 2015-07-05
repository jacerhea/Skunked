using System;

namespace Skunked.State.Events
{
    //should add an Guid?
    public class Event
    {
        public Event()
        {
            Occurred = DateTimeOffset.Now;
        }

        public DateTimeOffset Occurred { get; set; }
        public Guid GameId { get; set; }
    }
}
