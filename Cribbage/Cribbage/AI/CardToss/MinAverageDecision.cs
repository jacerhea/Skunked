using System.Collections.Generic;
using System.Linq;
using Cribbage.AI.CardToss;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Score;
using Games.Domain.MainModule.Entities.PlayingCards;
using MoreLinq;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss
{
    public class MinAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MinAverageDecision(IScoreCalculator scoreCalculator) : base(scoreCalculator)
        {}

        public IEnumerable<ICard> DetermineCardsToThrow(IEnumerable<ICard> hand)
        {
            IEnumerable<ComboPossibleScores> comboPossibleScoreses = BaseAverageDecision(hand);
            var lowestScoringCombo = comboPossibleScoreses.MinBy(cps => cps.GetScoreSummation());
            return hand.Where(c => !lowestScoringCombo.Combo.Contains(c));
        }
    }
}