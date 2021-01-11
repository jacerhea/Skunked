using System;
using System.Collections.Generic;

namespace Skunked.Cards.Value
{
    /// <summary>
    /// Returns value of a card.  Ace = 1, 9 = 9, face cards  = 10.
    /// </summary>
    public class AceLowFaceTenCardValueStrategy
    {
        private static readonly Dictionary<Rank, int> ValueLookup = new()
        {
            { Rank.Ace, 1 },
            { Rank.Two, 2 },
            { Rank.Three, 3 },
            { Rank.Four, 4 },
            { Rank.Five, 5 },
            { Rank.Six, 6 },
            { Rank.Seven, 7 },
            { Rank.Eight, 8 },
            { Rank.Nine, 9 },
            { Rank.Ten, 10 },
            { Rank.Jack, 10 },
            { Rank.Queen, 10 },
            { Rank.King, 10 }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public int GetValue(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            return ValueLookup[card.Rank];
        }
    }
}