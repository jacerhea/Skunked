using System.Collections.Generic;
using System.Linq;
using Skunked.Score.Interface;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away based on the summation of each combination possible with every possible cut card.
    /// </summary>
    public class MaxAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MaxAverageDecision(IScoreCalculator scoreCalculator = null) : base(scoreCalculator)
        {}

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var cards = hand as IList<Card> ?? hand.ToList();
            var comboPossibleScoreses = BaseAverageDecision(cards);
            var highestScoringCombo = comboPossibleScoreses.MaxBy(cps => cps.GetScoreSummation());
            return cards.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
 
    }
}