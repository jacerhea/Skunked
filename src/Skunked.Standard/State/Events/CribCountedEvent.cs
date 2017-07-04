namespace Skunked.State.Events
{
    public class CribCountedEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}