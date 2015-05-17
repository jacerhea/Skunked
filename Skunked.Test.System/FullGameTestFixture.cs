using System;
using System.Collections.Generic;
using System.Threading;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.Players;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System
{
    public class FullGameTestFixture : IDisposable
    {

        public FullGameTestFixture()
        {
            RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
        }

        [Fact]
        public void FullGameTest()
        {
            var game =
                new CribbageGameRunner(new List<Player>
                {
                    new Player("Player 1", 1, new MaxPlayStrategy(), new MaxAverageDecision()),
                    new Player("Player 2", 2, new MaxPlayStrategy(), new MaxAverageDecision())
                });

            var result = game.Run();
        }

        public void Dispose()
        {
            RandomProvider.ResetInstance();
        }
    }
}
