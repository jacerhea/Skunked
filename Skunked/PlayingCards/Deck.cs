using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Skunked.Utility;

namespace Skunked.PlayingCards
{
    public class Deck : IEnumerable<Card>
    {
        // Make a list.
        private readonly List<Card> _deck;

        public Deck()
        {
            var ranks = EnumHelper.GetValues<Rank>();
            var suits = EnumHelper.GetValues<Suit>();
            _deck = ranks.Cartesian(suits, (rank, suit) => new Card(rank, suit)).ToList();
        }

        public void Shuffle()
        {
            _deck.Shuffle();
        }

        public virtual IEnumerator<Card> GetEnumerator()
        {
            return _deck.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}