using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.Players;
using Skunked.Utility;

namespace Skunked.Test.System
{
    [TestClass]
    public class FullGameTestFixture
    {

        [TestInitialize]
        public void Setup()
        {
            RandomProvider.RandomInstance = new ThreadLocal<Random>(() => new IncrementalRandom());
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            RandomProvider.ResetInstance();
        }

        [TestMethod]
        public void FullGameTest()
        {
            var game =
                new CribbageGame(new List<Player>
                {
                    new Player("Player 1", 1, new MaxPlayStrategy(), new MaxAverageDecision()),
                    new Player("Player 2", 2, new MaxPlayStrategy(), new MaxAverageDecision())
                });

            var result = game.Run();
        }
    }
}
