namespace Skunked.State.Events
{
    public class CribCountedEvent : Event
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}