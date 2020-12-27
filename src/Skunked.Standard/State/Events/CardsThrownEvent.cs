using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.State.Events
{
    public class CardsThrownEvent : StreamEvent
    {
        public int PlayerId { get; set; }
        public List<Card> Thrown { get; set; } = new List<Card>();
    }
}