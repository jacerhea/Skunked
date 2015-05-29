using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;

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
            var handIter = new HashSet<Card>(hand);
            var combinations = new Combinations<Card>(handIter.ToList(), 4);
            var deck = new Deck();
            var possibleCardsCut = deck.Where(card => !handIter.Contains(card)).ToList();

            var comboPossibleScoreses = GetPossibleCombos(combinations, possibleCardsCut).ToList();
            return comboPossibleScoreses;
        }

        private IEnumerable<ComboPossibleScores> GetPossibleCombos(Combinations<Card> combinations, List<Card> possibleCardsCut)
        {
            foreach (var combo in combinations)
            {
                var possibleScores = possibleCardsCut.AsParallel().Select(cutCard => _scoreCalculator.CountShowScore(cutCard, combo).Score).ToList();
                yield return new ComboPossibleScores(combo) { PossibleScores = possibleScores };
            }
        }
    }
}