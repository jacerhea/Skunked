using System.Collections.Generic;

namespace Skunked.State.Events
{
    public class DeckShuffledEvent : Event
    {
        public List<Card> PostShuffleDeck { get; set; }
    }
}
