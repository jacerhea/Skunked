﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Skunked.Cards;
using Skunked.Game;
using Skunked.Players;
using Skunked.Rules;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System;

public class SmokeTests : IDisposable
{

    public SmokeTests()
    {
        //RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
    }

    [Fact]
    public void Run_2_Player_Game()
    {
        var game = new GameRunner(new Deck());

        var result = Parallel.ForEach(Enumerable.Range(0, 1000), index =>
        {
            var result = game.Run([
                new TestPlayer($"TestPlayer {index}", 1),
                new TestPlayer($"TestPlayer {index}_2", 2)
            ], WinningScoreType.Standard121);

            TestEndGame.Test(result.State);
        });

        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Run_4_Player_Game()
    {
        var game = new GameRunner(new Deck());

        var result = Parallel.ForEach(Enumerable.Range(0, 1000), index =>
        {
            var players = new List<IGameRunnerPlayer>
            {
                new TestPlayer($"TestPlayer {index}", 1),
                new TestPlayer($"TestPlayer {index}_2", 2),
                new TestPlayer($"TestPlayer {index}_3", 3),
                new TestPlayer($"TestPlayer {index}_4", 4),
            };
            var result = game.Run(players, WinningScoreType.Standard121);

            TestEndGame.Test(result.State);
        });

        result.IsCompleted.Should().BeTrue();
    }

    public void Dispose()
    {
        RandomProvider.ResetInstance();
    }
}