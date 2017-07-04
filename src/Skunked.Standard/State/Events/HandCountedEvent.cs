namespace Skunked.State.Events
{
    public class HandCountedEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}