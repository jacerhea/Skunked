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
            if (hand == null) throw new ArgumentNullException("hand");
            hand = hand.ToList();
            if (hand.Any(c => c.Rank == Rank.Four))
                return hand.First(c => c.Rank == Rank.Four);

            if (hand.Any(c => c.Rank == Rank.Three))
                return hand.First(c => c.Rank == Rank.Three);

            if (hand.Any(c => c.Rank == Rank.Two))
                return hand.First(c => c.Rank == Rank.Two);

            if (hand.Any(c => c.Rank == Rank.Ace))
                return hand.First(c => c.Rank == Rank.Ace);

            if (hand.Any(c => c.Rank != Rank.Five))
                return hand.First(c => c.Rank != Rank.Five);

            return hand.First();
        }
    }
}
