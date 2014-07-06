using System.Collections.Generic;
using System.Linq;
using Skunked.Utility;

namespace Skunked.PlayingCards
{
    public class Deck
    {
        // Make a list.
        protected readonly List<Card> _deck;

        public Deck()
        {
            var ranks = EnumHelper.GetValues<Rank>();
            var suits = EnumHelper.GetValues<Suit>();
            _deck = ranks.Cartesian(suits, (rank, suit) => new Card(rank, suit)).ToList();
        }

        public virtual IEnumerable<Card> Cards
        {
            get { return _deck; }
        }


        public virtual void Shuffle()
        {
            _deck.Shuffle();
        }
    }
}