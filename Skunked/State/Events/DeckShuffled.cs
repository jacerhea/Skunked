using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class DeckShuffled
    {
        public List<Card> ShuffledDeck { get; set; }
    }
}
