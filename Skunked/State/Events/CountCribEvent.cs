namespace Skunked.State.Events
{
    public class CountCribEvent : Event
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}