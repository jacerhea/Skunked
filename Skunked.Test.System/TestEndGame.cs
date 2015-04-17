using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Test.System
{
    public class TestEndGame
    {
        public static void Test(GameState gameState)
        {
            foreach (var teamScore in gameState.TeamScores)
            {
                Assert.AreEqual(teamScore.Score, gameState.IndividualScores.Where(s => teamScore.Players.Contains(s.Player)).Sum(ps => ps.Score));
            }   
         
            Assert.IsTrue(gameState.TeamScores.Count(ts => ts.Score >= gameState.GameRules.WinningScore) == 1);

            Assert.IsTrue(gameState.IsGameFinished());
            Assert.IsTrue(gameState.Rounds.All(r => r.DealtCards.Select(p => p.Hand).All(cards => cards.Distinct().Count() == gameState.GameRules.HandSizeToDeal)));
            Assert.IsTrue(gameState.Rounds.All(r => r.Hands.Select(p => p.Hand).All(cards => cards.Distinct().Count() == gameState.GameRules.HandSize)));
        }
    }
}
