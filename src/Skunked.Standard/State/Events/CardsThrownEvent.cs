using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class CardsThrownEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public List<Card> Thrown { get; set; }
    }
}