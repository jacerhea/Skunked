using System;

namespace Cribbage
{
    public class Card : IEquatable<Card>
    {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public Card(Rank rank = Rank.Ace , Suit suit = Suit.Clubs)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", Rank, Suit);
        }

        public bool Equals(Card other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Rank == Rank && other.Suit == Suit;
        }
    }
}