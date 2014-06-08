using System.Collections.Generic;
using System.Linq;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    public class RandomDecision : IDecisionStrategy
    {
        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var handCopy = hand.ToList();

            while (handCopy.Count > 4)
            {
                var indexToDrop = RandomProvider.GetThreadRandom().Next(0, handCopy.Count() - 1);
                var cardToDrop = handCopy[indexToDrop];
                handCopy.RemoveAt(indexToDrop);
                yield return cardToDrop;
            }
        }
    }
}