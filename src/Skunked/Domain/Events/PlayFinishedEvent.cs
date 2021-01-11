namespace Skunked.Domain.Events
{
    public class PlayFinishedEvent : StreamEvent
    {
        public int Round { get; set; }
    }
}
