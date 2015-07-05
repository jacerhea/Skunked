namespace Skunked.State.Events
{
    public class HandCountedEvent : Event
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}