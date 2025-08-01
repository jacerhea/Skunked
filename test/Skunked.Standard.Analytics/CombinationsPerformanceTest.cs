﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Skunked.Cards;
using Skunked.Score;

namespace Skunked.Benchmarks;

[SimpleJob(RuntimeMoniker.NetCoreApp50)]
[HtmlExporter]
public class ScoreCalculator_Benchmarks
{
    private readonly List<Card> _hand;
    private readonly Card _starter;
    private readonly List<int> _continuous = Enumerable.Range(0, 100).ToList();
    private readonly List<int> _nonContinuous;
    private readonly ScoreCalculator _scoreCalculator = new();

    public ScoreCalculator_Benchmarks()
    {
        _hand =
        [
            new(Rank.Six, Suit.Clubs),
            new(Rank.Eight, Suit.Diamonds),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Queen, Suit.Spades)
        ];
        _starter = new Card(Rank.Seven, Suit.Diamonds);
        _nonContinuous = Enumerable.Range(0, 100).ToList();
        _nonContinuous.RemoveAt(50);
    }

    [Benchmark]
    public void CountShowPoints()
    {
        _scoreCalculator.CountShowPoints(_starter, _hand);
    }

    [Benchmark]
    public void AreContinuous()
    {
        _scoreCalculator.IsContinuous(_continuous);
    }

    [Benchmark]
    public void AreNonContinuous()
    {
        _scoreCalculator.IsContinuous(_nonContinuous);
    }

    [Benchmark]
    public void GetCombinations()
    {
        _scoreCalculator.GetCombinations(_hand.Append(_starter).ToList());
    }
}