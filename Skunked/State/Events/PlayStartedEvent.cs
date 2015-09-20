namespace Skunked.State.Events
{
    public class PlayStartedEvent : StreamEvent
    {
        public int Round { get; set; }
    }
}
