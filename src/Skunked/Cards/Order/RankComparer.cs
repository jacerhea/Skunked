using System;
using System.Collections.Generic;

namespace Skunked.Cards.Order
{
    /// <summary>
    /// Compare by value of the Cards rank only.
    /// </summary>
    public class RankComparer : IComparer<Card>
    {
        /// <summary>
        /// 
        /// </summary>
        public static RankComparer Instance => new();

        /// <inheritdoc />
        public int Compare(Card? x, Card? y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return x.Rank.CompareTo(y.Rank);
        }
    }
}