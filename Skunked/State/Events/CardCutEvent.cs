using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class CardCutEvent : Event
    {
        public int PlayerId { get; set; }
        public Card CutCard { get; set; }
    }
}