using System.Collections.Generic;
using System.Linq;
using Cribbage.Utility;

namespace Cribbage.PlayingCards
{
    public class Deck
    {
        // Make a list.
        private readonly List<Card> _deck;
        public Deck()
        {
            var ranks = EnumHelper.GetValues<Rank>();
            var suits = new List<Suit> { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
            _deck = ranks.Cartesian(suits, (rank, suit) => new Card(rank, suit)).ToList();
        }

        public List<Card> Cards
        {
            get { return _deck; }
        }
    }
}