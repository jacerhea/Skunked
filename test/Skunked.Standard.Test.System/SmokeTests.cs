using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Skunked.Cards;
using Skunked.Game;
using Skunked.Players;
using Skunked.Rules;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System
{
    public class SmokeTests : IDisposable
    {

        public SmokeTests()
        {
            RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
        }

        [Fact]
        public void FullGameTest()
        {
            var game = new GameRunner(new Deck());

            foreach (var i in Enumerable.Range(0, 1000))
            {
                var result = game.Run(new List<IGameRunnerPlayer>
                {
                    new TestPlayer($"TestPlayer {i}", 1),
                    new TestPlayer($"TestPlayer {i}_2", 2)
                }, new GameRules());

                TestEndGame.Test(result.State);
            }
        }

        public void Dispose()
        {
            RandomProvider.ResetInstance();
        }
    }
}
