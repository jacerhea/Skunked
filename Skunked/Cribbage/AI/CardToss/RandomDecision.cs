using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss
{
    public class RandomDecision : IDecisionStrategy
    {
        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var handCopy = hand.ToList();

            while (handCopy.Count > 4)
            {
                var randomGen = new Random();
                var indexToDrop = randomGen.Next(0, handCopy.Count() - 1);
                var cardToDrop = handCopy[indexToDrop];
                handCopy.RemoveAt(indexToDrop);
                yield return cardToDrop;
            }
        }
    }
}