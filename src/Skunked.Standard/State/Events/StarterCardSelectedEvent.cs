using Skunked.Cards;

namespace Skunked.State.Events
{
    public class StarterCardSelectedEvent : StreamEvent
    {
        public Card Starter { get; set; }
    }
}
