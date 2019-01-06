using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away based on the summation of each combination possible with every possible cut card.
    /// </summary>
    public class MaxAverageDecision : AbstractAverageDecision, IDecisionStrategy
    {
        public MaxAverageDecision(ScoreCalculator scoreCalculator = null)
            : base(scoreCalculator)
        { }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var cards = hand as IList<Card> ?? hand.ToList();
            var comboPossibleScoreses = BaseAverageDecision(cards);
            var highestScoringCombo = comboPossibleScoreses.MaxBy(cps => cps.GetScoreSummation());
            //throw the cards that are not part of the highest scoring combo.
            return cards.Except(highestScoringCombo.Combo);
        }
    }
}