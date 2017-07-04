using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    public class MinAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MinAverageDecision(ScoreCalculator scoreCalculator = null)
            : base(scoreCalculator)
        { }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand));
            var handCopy = hand.ToList();
            var comboPossibleScoreses = BaseAverageDecision(handCopy);
            var lowestScoringCombo = comboPossibleScoreses.MinBy(cps => cps.GetScoreSummation());
            return handCopy.Where(c => !lowestScoringCombo.Combo.Contains(c));
        }
    }
}