using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class PlayCardEvent : Event
    {
        public int PlayerId { get; set; }
        public Card PlayedCard { get; set; }
    }
}