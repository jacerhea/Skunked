using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class CardPlayedEvent : Event
    {
        public int PlayerId { get; set; }
        public Card PlayedCard { get; set; }
    }
}