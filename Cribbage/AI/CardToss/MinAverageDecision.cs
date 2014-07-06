using System.Collections.Generic;
using System.Linq;
using Skunked.Score;
using Skunked.Score.Interface;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    public class MinAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MinAverageDecision(IScoreCalculator scoreCalculator = null) : base(scoreCalculator)
        {}

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            IEnumerable<ComboPossibleScores> comboPossibleScoreses = BaseAverageDecision(hand);
            var lowestScoringCombo = comboPossibleScoreses.MinBy(cps => cps.GetScoreSummation());
            return hand.Where(c => !lowestScoringCombo.Combo.Contains(c));
        }
    }
}