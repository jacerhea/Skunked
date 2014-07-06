using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Skunked.PlayingCards;

namespace Skunked
{
    public class Card : IEquatable<Card>, ISerializable , IEqualityComparer<Card>
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

        public override bool Equals(object obj)
        {
            if (obj == null) throw new ArgumentNullException("other");
            var card = (Card) obj;
            return card.Rank == Rank && card.Suit == Suit;
        }

        public override int GetHashCode()
        {
            return (int)Suit ^ (int)Rank;
        }

        public bool Equals(Card other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Rank == Rank && other.Suit == Suit;
        }

        public bool Equals(Card x, Card y)
        {
            return x.Rank == y.Rank && x.Suit == y.Suit;
        }

        public int GetHashCode(Card obj)
        {
            return (int)obj.Suit ^ (int)obj.Rank;
        }
    }

    public class CardValueEquality : IEqualityComparer<Card>
    {
        static CardValueEquality()
        {
            Instance = new CardValueEquality();
        }

        public static CardValueEquality Instance { get; private set; }

        public bool Equals(Card x, Card y)
        {
            return x.Rank == y.Rank && x.Suit == y.Suit;
        }

        public int GetHashCode(Card obj)
        {
            return (int)obj.Suit ^ (int)obj.Rank;
        }
    }
}