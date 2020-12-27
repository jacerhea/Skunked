using Skunked.Cards;

namespace Skunked.State.Events
{
    public class CardCutEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public Card CutCard { get; set; }
    }
}