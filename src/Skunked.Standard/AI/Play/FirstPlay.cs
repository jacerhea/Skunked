using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;

namespace Skunked.AI.Play
{
    public abstract class FirstPlay
    {
        protected Card PlayFirstCard(IEnumerable<Card> hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand));
            var handCopy = hand.ToList();
            if (handCopy.Any(c => c.Rank == Rank.Four))
                return handCopy.First(c => c.Rank == Rank.Four);

            if (handCopy.Any(c => c.Rank == Rank.Three))
                return handCopy.First(c => c.Rank == Rank.Three);

            if (handCopy.Any(c => c.Rank == Rank.Two))
                return handCopy.First(c => c.Rank == Rank.Two);

            if (handCopy.Any(c => c.Rank == Rank.Ace))
                return handCopy.First(c => c.Rank == Rank.Ace);

            if (handCopy.Any(c => c.Rank != Rank.Five))
                return handCopy.First(c => c.Rank != Rank.Five);

            return handCopy.First();
        }
    }
}
