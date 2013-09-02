using System.Collections.Generic;

namespace Cribbage.AI.CardToss
{
    public interface IDecisionStrategy
    {
        /// <summary>
        /// Take a dealt cribbage hand and return which cards should be thrown.
        /// </summary>
        /// <param name="hand">dealt hand</param>
        /// <returns>cards to throw</returns>
        IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand);
    }
}