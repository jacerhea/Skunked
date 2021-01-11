﻿using System;
using System.Collections.Generic;

namespace Skunked.Cards
{
    /// <summary>
    /// Standard 52-card deck playing card.
    /// </summary>
    public class Card : IEquatable<Card>, IEqualityComparer<Card>
    {
        /// <summary>
        /// Playing card's rank.
        /// </summary>
        public Rank Rank { get; }
        
        /// <summary>
        /// Playing card's suit.
        /// </summary>
        public Suit Suit { get; }

        /// <summary>
        /// Initializes a new instance of Card with default Rank and Suit.
        /// </summary>
        public Card(): this(Rank.Ace, Suit.Clubs) { }

        /// <summary>
        /// Initializes a new instance of Card with optional Rank and Suit
        /// </summary>        
        public Card(Rank rank = Rank.Ace, Suit suit = Suit.Clubs)
        {
            Rank = rank;
            Suit = suit;
        }

        /// <summary>
        /// Initializes a new instance of Card from existing Card.
        /// </summary>           
        public Card(Card card)
        {
            Rank = card.Rank;
            Suit = card.Suit;
        }

        /// <summary>
        /// Returns a string representation of the Card.
        /// </summary>   
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }

        /// <summary>
        /// True if the object has same value as Card.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            var card = (Card)obj;
            return card.Rank == Rank && card.Suit == Suit;
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)Suit ^ (int)Rank;
        }

        /// <summary>
        /// Checks if the object has same value as Card.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if objects are equal.</returns>        
        public bool Equals(Card other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Rank == Rank && other.Suit == Suit;
        }

        /// <summary>
        /// Checks if objects are equal.
        /// </summary>
        /// <param name="x">Card 1.</param>
        /// <param name="y">Card 2.</param>
        /// <returns>True if objects are equal.</returns>
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