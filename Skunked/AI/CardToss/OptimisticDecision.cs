using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away in a hand of Cribbage based on the highest possible score of all possibly cut cards.
    /// </summary>
    public class OptimisticDecision : IDecisionStrategy
    {
        private readonly ScoreCalculator _scoreCalculator;
        private readonly Deck _deck;


        public OptimisticDecision(ScoreCalculator scoreCalculator = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _deck = new Deck();
        }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var handList = hand.ToList();
            var combinations = new Combinations<Card>(handList, 4);

            var possibleCardsCut = _deck.Where(card => !handList.Contains(card)).ToList();

            var totalPossibleCombinations = combinations
                .SelectMany(combo => possibleCardsCut.Select(cutCard =>
                {
                    var possibleCombo = new List<Card>(combo) {cutCard};
                    var comboScore = _scoreCalculator.CountShowScore(cutCard, combo);
                    return new ComboScore(possibleCombo, comboScore.Score);
                }));

            var highestScoringCombo = totalPossibleCombinations.MaxBy(cs => cs.Score);
            return handList.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
    }
}