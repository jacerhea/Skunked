using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTestFixture
    {
        [TestMethod]
        public void TestCardProperties()
        {
            foreach (var source in Enumerable.Range(0, 100))
            {
                var game = new CribbageGame(new GameRules(), new List<Player> { new Player(), new Player() }, new Deck(), new ScoreCalculator());
                var result = game.Run();
                Assert.IsTrue(result.IsGameFinished());                
            }
        }
    }
}
