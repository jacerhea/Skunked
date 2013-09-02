using System;
using System.Collections.Generic;
using System.Linq;

namespace Cribbage.AI.CardToss
{
    public class RandomDecision : IDecisionStrategy
    {
        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var handCopy = hand.ToList();
            var randomGen = new Random();

            while (handCopy.Count > 4)
            {
                var indexToDrop = randomGen.Next(0, handCopy.Count() - 1);
                var cardToDrop = handCopy[indexToDrop];
                handCopy.RemoveAt(indexToDrop);
                yield return cardToDrop;
            }
        }
    }
}