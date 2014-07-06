using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Dealer;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestCardProperties()
        {
            var game = new CribbageGame(new GameRules(), new List<Player>{new Player(), new Player()}, new Deck(), new ScoreCalculator(), new StandardHandDealer());
            game.Run();
        }
    }
}
