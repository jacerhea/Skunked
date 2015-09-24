using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class StarterCardSelectedEvent : StreamEvent
    {
        public Card Starter { get; set; }
    }
}
