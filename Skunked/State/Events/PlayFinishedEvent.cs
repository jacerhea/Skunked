namespace Skunked.State.Events
{
    public class PlayFinishedEvent : StreamEvent
    {
        public int Round { get; set; }
    }
}
