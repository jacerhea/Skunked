namespace Skunked.Domain.Events
{
    public class PlayStartedEvent : StreamEvent
    {
        public int Round { get; set; }
    }
}
