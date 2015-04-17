namespace Skunked.State.Events
{
    public class CutCardEvent : Event
    {
        public int PlayerId { get; set; }
        public Card CutCard { get; set; }
    }
}