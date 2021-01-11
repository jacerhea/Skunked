using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.Cards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Benchmarks
{
    public class DistributionService
    {
        private readonly ScoreCalculator _scoreCalculator;

        public DistributionService(ScoreCalculator scoreCalculator = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        public Distribution CalculateDistribution(List<Card> hand)
        {
            var handCopy = hand.ToList();
            var combinations = new Combinations<Card>(handCopy, 4);
            var deck = new Deck();
            var possibleCardsCut = deck.Where(card => !handCopy.Contains(card)).ToList();


            var comboPossibleScorings = new List<ComboPossibleScores>((int)combinations.Count);

            foreach (var combo in combinations)
            {
                var possibleScores = possibleCardsCut.Select(cutCard => new ScoreWithCut { Cut = cutCard, Score = _scoreCalculator.CountShowPoints(cutCard, combo).Points.Score }).ToList();
                var comboPossibleScores = new ComboPossibleScores(combo, possibleScores);
                comboPossibleScorings.Add(comboPossibleScores);
            }

            var distributionSets = comboPossibleScorings.SelectMany(cps => cps.PossibleScores)
                .GroupBy(cps =>  cps.Score)
                .Select(g => new DistributionSet { Count = g.Count(), Score = g.Key })
                .OrderBy(ds => ds.Score)
                .ToList();

            var median = distributionSets.Select(ds => ds.Score).Distinct().OrderBy(s => s).ToList();
            var resultCount = distributionSets.Sum(ds => ds.Count);
            var mean = (decimal)distributionSets.Sum(ds => ds.Score * ds.Count) / resultCount;

            var squaredDifferences = distributionSets.Sum(ds => Math.Pow(Math.Abs((double)mean - ds.Score), 2) * ds.Count);
            return new Distribution
            {
                Sets = distributionSets,
                Mean = mean,
                Median = median[(int)Math.Floor(median.Count / 2M)],
                Mode = distributionSets.MaxBy(ds => ds.Count).Score,
                Range = new Range<int> { Upper = distributionSets.MaxBy(ds => ds.Score).Score, Lower = distributionSets.MinBy(ds => ds.Score).Score },
                StandardDeviation = Math.Sqrt(squaredDifferences / resultCount),
                BestCut = comboPossibleScorings.SelectMany(spc => spc.PossibleScores).MaxBy(ps => ps.Score).Cut
            };
        }
    }

    public class Distribution
    {
        public List<DistributionSet> Sets { get; set; }
        public decimal Mean { get; set; }
        public int Median { get; set; }
        public int Mode { get; set; }
        public Range<int> Range { get; set; }
        public double StandardDeviation { get; set; }
        public Card BestCut { get; set; }
    }

    public class DistributionSet
    {
        public int Score { get; set; }
        public int Count { get; set; }
    }

    public class Range<T>
    {
        public T Upper { get; set; }
        public T Lower { get; set; }
    }
}
