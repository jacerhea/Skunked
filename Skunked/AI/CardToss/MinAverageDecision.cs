using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    public class MinAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MinAverageDecision(IScoreCalculator scoreCalculator = null)
            : base(scoreCalculator)
        { }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var cards = hand as IList<Card> ?? hand.ToList();
            IEnumerable<ComboPossibleScores> comboPossibleScoreses = BaseAverageDecision(cards);
            var lowestScoringCombo = comboPossibleScoreses.MinBy(cps => cps.GetScoreSummation());
            return cards.Where(c => !lowestScoringCombo.Combo.Contains(c));
        }
    }
}