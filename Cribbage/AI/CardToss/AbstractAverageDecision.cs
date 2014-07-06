using System.Collections.Generic;
using System.Linq;
using Skunked.Combinatorics;
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
            hand = hand.ToList();
            var combinations = new Combinations<Card>(hand.ToList(), 4);
            var deck = new List<Card>();
            var possibleCardsCut = deck.Where(card => !hand.Contains(card)).ToList();

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