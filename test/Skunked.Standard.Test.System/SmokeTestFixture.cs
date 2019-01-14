using System;
using System.Collections.Generic;
using System.Threading;
using Skunked.Game;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System
{
    public class SmokeTestFixture : IDisposable
    {

        public SmokeTestFixture()
        {
            RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
        }

        [Fact]
        public void FullGameTest()
        {
            var game =
                new CribbageGameRunner(new Deck());

            var result = game.Run(new List<IGameRunnerPlayer>
            {
                new TestPlayer("TestPlayer 1", 1),
                new TestPlayer("TestPlayer 2", 2)
            }, new GameRules());
        }

        public void Dispose()
        {
            RandomProvider.ResetInstance();
        }
    }
}
