using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Score.Interface;

namespace Skunked.AI.CardToss
{
    public abstract class AbstractAverageDecision
    {
        private readonly IScoreCalculator _scoreCalculator;

        protected AbstractAverageDecision(IScoreCalculator scoreCalculator = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        protected IEnumerable<ComboPossibleScores> BaseAverageDecision(IEnumerable<Card> hand)
        {
            var handIter = hand.ToList();
            var combinations = new Combinations<Card>(handIter, 4);
            var deck = new Deck();
            var possibleCardsCut = deck.Where(card => !handIter.Contains(card)).ToList();

            var comboPossibleScoreses = new List<ComboPossibleScores>();

            foreach (var combo in combinations)
            {
                var comboPossibleScores = new ComboPossibleScores(combo);
                comboPossibleScoreses.Add(comboPossibleScores);

                foreach (var cutCard in possibleCardsCut)
                {
                    var comboScore = _scoreCalculator.CountShowScore(cutCard, combo).Score;
                    comboPossibleScores.AddScore(comboScore);
                }
            }

            return comboPossibleScoreses;
        }
    }
}