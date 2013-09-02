using System.Collections.Generic;
using System.Linq;
using Cribbage.Score.Interface;
using Cribbage.Utility;

namespace Cribbage.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away based on the summation of each combination possible with every possible cut card.
    /// </summary>
    public class MaxAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MaxAverageDecision(IScoreCalculator scoreCalculator) : base(scoreCalculator)
        {}

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var comboPossibleScoreses = BaseAverageDecision(hand);
            var highestScoringCombo = comboPossibleScoreses.MaxBy(cps => cps.GetScoreSummation());
            return hand.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
    }
}