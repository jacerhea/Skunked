using System.Collections.Generic;
using System.Linq;
using Cribbage.AI.CardToss;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Score;
using Games.Domain.MainModule.Entities.PlayingCards;
using MoreLinq;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away based on the summation of each combination possible with every possible cut card.
    /// </summary>
    public class MaxAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MaxAverageDecision(IScoreCalculator scoreCalculator) : base(scoreCalculator)
        {}

        public IEnumerable<ICard> DetermineCardsToThrow(IEnumerable<ICard> hand)
        {
            var comboPossibleScoreses = BaseAverageDecision(hand);
            var highestScoringCombo = comboPossibleScoreses.MaxBy(cps => cps.GetScoreSummation());
            return hand.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
    }
}