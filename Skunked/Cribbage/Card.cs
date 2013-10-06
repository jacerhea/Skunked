using System;
using System.Runtime.Serialization;

namespace Cribbage
{
    public class Card : IEquatable<Card>, ISerializable 
    {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }

        public Card(){}

        public Card(Rank rank = Rank.Ace , Suit suit = Suit.Clubs)
        {
            Rank = rank;
            Suit = suit;
        }

        public Card(Card card)
        {
            Rank = card.Rank;
            Suit = card.Suit;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", Rank, Suit);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Rank = (Rank) Enum.Parse(typeof(Rank), info.GetString("Rank"));
            Suit = (Suit)Enum.Parse(typeof(Suit), info.GetString("Suit"));
        }

        public bool Equals(Card other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Rank == Rank && other.Suit == Suit;
        }
    }
}