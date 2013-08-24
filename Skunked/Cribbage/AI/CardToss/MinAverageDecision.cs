using System.Collections.Generic;
using System.Linq;
using Cribbage;
using Cribbage.AI.CardToss;
using Cribbage.Score;
using Cribbage.Score.Interface;
using Cribbage.Utility;


namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss
{
    public class MinAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MinAverageDecision(IScoreCalculator scoreCalculator) : base(scoreCalculator)
        {}

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            IEnumerable<ComboPossibleScores> comboPossibleScoreses = BaseAverageDecision(hand);
            var lowestScoringCombo = comboPossibleScoreses.MinBy(cps => cps.GetScoreSummation());
            return hand.Where(c => !lowestScoringCombo.Combo.Contains(c));
        }
    }
}