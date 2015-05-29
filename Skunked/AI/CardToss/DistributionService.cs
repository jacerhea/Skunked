using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.CardToss
{
    public class DistributionService
    {
        private readonly IScoreCalculator _scoreCalculator;

        public DistributionService(IScoreCalculator scoreCalculator = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        public Distribution CalculateDistribution(List<Card> hand)
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
                var possibleScores = possibleCardsCut.AsParallel().Select(cutCard => _scoreCalculator.CountShowScore(cutCard, combo).Score);
                comboPossibleScores.PossibleScores.AddRange(possibleScores);
            }

            var distributionSets = comboPossibleScoreses.SelectMany(cps => cps.PossibleScores)
                .GroupBy(cps => cps)
                .Select(g => new DistributionSet { Count = g.Count(), Score = g.Key })
                .OrderBy(ds => ds.Score)
                .ToList();

            return new Distribution { Sets = distributionSets, Mean = (decimal)distributionSets.Sum(ds => ds.Score * ds.Count) / distributionSets.Sum(ds => ds.Count), Mode = distributionSets.MaxBy(ds => ds.Count).Score};
        }
    }

    public class Distribution
    {
        public List<DistributionSet> Sets { get; set; }
        public decimal Mean { get; set; }
        public int Median { get; set; }
        public int Mode { get; set; }
    }

    public class DistributionSet
    {
        public int Score { get; set; }
        public int Count { get; set; }
    }
}
