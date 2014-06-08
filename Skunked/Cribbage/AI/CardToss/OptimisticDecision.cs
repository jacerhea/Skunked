using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Combinatorics;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Score.Interface;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away in a hand of Cribbage based on the highest possible score of all possibly cut cards.
    /// </summary>
    public class OptimisticDecision : IDecisionStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly Deck _deck;
        

        public OptimisticDecision(IScoreCalculator scoreCalculator)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            _scoreCalculator = scoreCalculator;
            _deck = new Deck();
        }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var handList = hand.ToList();
            var combinations = new Combinations<Card>(handList, 4);

            var possibleCardsCut = _deck.Cards.Where(card => !handList.Contains(card)).ToList();

            var totalPossibleCombinations = new List<ComboScore>();

            foreach (var combo in combinations)
            {
                foreach (var cutCard in possibleCardsCut)
                {
                    var possibleCombo = new List<Card>(combo) { cutCard };
                    var comboScore = _scoreCalculator.CountShowScore(cutCard, combo);

                    totalPossibleCombinations.Add(new ComboScore(possibleCombo, comboScore.Score));
                }
            }

            var highestScoringCombo = totalPossibleCombinations.MaxBy(cs => cs.Score);
            return handList.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
    }
}