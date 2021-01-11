using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Skunked.Utility;

namespace Skunked.Cards
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Deck : IEnumerable<Card>
    {
        private static readonly ImmutableList<Card> InitialDeck = EnumHelper.GetValues<Rank>()
            .Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new Card(rank, suit)).ToImmutableList();
        private readonly List<Card> _deck;

        /// <summary>
        /// Initializes a deck with 52 cards.
        /// </summary>
        public Deck()
        {
            _deck = InitialDeck.ToList();
        }

        /// <summary>
        /// Initializes a deck with the given cards.
        /// </summary>
        /// <param name="deck"></param>
        public Deck(IEnumerable<Card> deck)
        {
            _deck = deck.ToList();
        }

        /// <summary>
        /// Randomly shuffles the deck. 
        /// </summary>
        public void Shuffle()
        {
            _deck.Shuffle();
        }

        
        /// <summary>
        /// Randomly shuffles the deck the given number of times. 
        /// </summary>        
        public void Shuffle(int count)
        {
            foreach (var _ in Enumerable.Range(1, count))
            {
                _deck.Shuffle();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Deck
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Card> GetEnumerator()
        {
            return _deck.GetEnumerator();
        }

        /// Returns an enumerator that iterates through the Deck
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}