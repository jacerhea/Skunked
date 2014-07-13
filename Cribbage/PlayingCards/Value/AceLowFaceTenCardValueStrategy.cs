using System;
using System.Collections.Generic;

namespace Skunked.PlayingCards.Value
{
    public class AceLowFaceTenCardValueStrategy : ICardValueStrategy
    {
        private static readonly Dictionary<Rank, int> ValueLookup = new Dictionary<Rank, int>
        {
            {Rank.Ace, 1},
            {Rank.Two, 2},
            {Rank.Three, 3},
            {Rank.Four, 4},
            {Rank.Five, 5},
            {Rank.Six, 6},
            {Rank.Seven, 7},
            {Rank.Eight, 8},
            {Rank.Nine, 9},
            {Rank.Ten, 10},
            {Rank.Jack, 10},
            {Rank.Queen, 10},
            {Rank.King, 10}
        };

        public int ValueOf(Card card)
        {
            return ValueLookup[card.Rank];
        }
    }
}