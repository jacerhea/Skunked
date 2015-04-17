namespace Skunked.State.Events
{
    public class CountHandEvent : Event
    {
        public int PlayerId { get; set; }
        public int CountedScore { get; set; }
    }
}