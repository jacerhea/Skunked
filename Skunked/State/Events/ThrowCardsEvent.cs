using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class ThrowCardsEvent : Event
    {
        public int PlayerId { get; set; }
        public List<Card> Thrown { get; set; }
    }
}