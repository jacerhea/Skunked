using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage;
using Cribbage.Score;
using Cribbage.Score.Interface;


namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.AI.CardToss
{
    /// <summary>
    /// Returns cards to throw away in a hand of Cribbage based on the highest possible score of all possibly cut cards.
    /// </summary>
    public class OptimisticDecision : IDecisionStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;

        public OptimisticDecision(IScoreCalculator scoreCalculator)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            _scoreCalculator = scoreCalculator;
        }

        public IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
        {
            var combinations = new Combinations<Card>(hand.ToList(), 4);

            var deck = new Standard52CardDeck();
            var possibleCardsCut = deck.Cards.Where(card => !hand.Contains(card));

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

            var highestScore = totalPossibleCombinations.Max(cs => cs.Score);
            var highestScoringCombo = totalPossibleCombinations.First(cs => cs.Score == highestScore);
            return hand.Where(card => !highestScoringCombo.Combo.Contains(card));
        }
    }
}