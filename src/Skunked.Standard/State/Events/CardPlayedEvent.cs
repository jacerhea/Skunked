using Skunked.Cards;

namespace Skunked.State.Events
{
    public class CardPlayedEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public Card Played { get; set; }
    }
}