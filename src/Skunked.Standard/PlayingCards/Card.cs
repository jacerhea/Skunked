using System;
using System.Collections.Generic;

namespace Skunked.PlayingCards
{
    public class Card : IEquatable<Card>, IEqualityComparer<Card>
    {
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }

        public Card(): this(Rank.Ace, Suit.Clubs) { }

        public Card(Rank rank = Rank.Ace, Suit suit = Suit.Clubs)
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
            return $"{Rank} of {Suit}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var card = (Card)obj;
            return card.Rank == Rank && card.Suit == Suit;
        }

        public override int GetHashCode()
        {
            return (int)Suit ^ (int)Rank;
        }

        public bool Equals(Card other)
        {
            if (other == null)
            {
                return false;
            }
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