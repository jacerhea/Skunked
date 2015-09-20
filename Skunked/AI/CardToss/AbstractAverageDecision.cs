using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;

namespace Skunked.AI.CardToss
{
    public abstract class AbstractAverageDecision
    {
        private readonly ScoreCalculator _scoreCalculator;

        protected AbstractAverageDecision(ScoreCalculator scoreCalculator = null)
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

        private IEnumerable<ComboPossibleScores> GetPossibleCombos(Combinations<Card> handCombinations, List<Card> possibleStarterCards)
        {
            return handCombinations.AsParallel().Select(combo =>
            {
                var possibleScores = possibleStarterCards
                    .Select(cutCard => new ScoreWithCut { Cut = cutCard, Score = _scoreCalculator.CountShowScore(cutCard, combo).Score })
                    .ToList();
                return new ComboPossibleScores(combo, possibleScores);
            });
        }
    }
}