using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Combinatorics.Collections;
using Skunked.AI;
using Skunked.Cards;
using Skunked.Score;

namespace Skunked.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [HtmlExporter]
    public class CardToss_Benchmarks
    {
        private readonly List<Card> _hand;
        private readonly Card _starter;
        private readonly List<int> _continuous = Enumerable.Range(0, 100).ToList();
        private readonly List<int> _nonContinuous;
        private readonly ScoreCalculator _scoreCalculator = new();

        public CardToss_Benchmarks()
        {
            _hand = new List<Card>
            {
                new(Rank.Six, Suit.Clubs),
                new(Rank.Eight, Suit.Diamonds),
                new(Rank.Two, Suit.Hearts),
                new(Rank.Queen, Suit.Spades)
            };
            _starter = new Card(Rank.Seven, Suit.Diamonds);
            _nonContinuous = Enumerable.Range(0, 100).ToList();
            _nonContinuous.RemoveAt(50);
        }

        [Benchmark]
        public void maxAverage()
        {
            var result = CardToss.maxAverage(_hand).ToList();
        }

        [Benchmark]
        public void optimisticDecision()
        {
            var result = CardToss.optimisticDecision(_hand).ToList();
        }


    }
}

