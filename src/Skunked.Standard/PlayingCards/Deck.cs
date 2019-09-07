using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Skunked.Utility;

namespace Skunked.PlayingCards
{
    public class Deck : IEnumerable<Card>
    {
        private static readonly ImmutableList<Card> InitialDeck = EnumHelper.GetValues<Rank>()
            .Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new Card(rank, suit)).ToImmutableList();
        private readonly List<Card> _deck;

        public Deck()
        {
            _deck = InitialDeck.ToList();
        }

        public void Shuffle()
        {
            _deck.Shuffle();
        }

        public void Shuffle(int count)
        {
            foreach (var i in Enumerable.Range(1, count))
            {
                _deck.Shuffle();
            }
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