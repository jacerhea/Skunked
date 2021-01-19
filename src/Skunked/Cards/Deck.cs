using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Skunked.Utility;

namespace Skunked.Cards
{
    /// <summary>
    ///
    /// </summary>
    public sealed class Deck : IEnumerable<Card>
    {
        private static readonly List<Card> InitialDeck = EnumHelper.GetValues<Rank>()
            .Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new Card(rank, suit)).ToList();

        private readonly List<Card> _deck;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class with 52 cards.
        /// </summary>
        public Deck()
        {
            _deck = InitialDeck.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class with 52 cards.
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
        /// <param name="count"></param>
        public void Shuffle(int count)
        {
            foreach (var _ in Enumerable.Range(1, count))
            {
                _deck.Shuffle();
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Card> GetEnumerator()
        {
            return _deck.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}