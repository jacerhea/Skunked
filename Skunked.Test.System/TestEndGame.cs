using System.Linq;
using Skunked.Rules;
using Skunked.State;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.System
{
    public class TestEndGame
    {
        public static void Test(GameState gameState)
        {
            foreach (var teamScore in gameState.TeamScores)
            {
                Assert.Equal(teamScore.Score, gameState.IndividualScores.Where(s => teamScore.Players.Contains(s.Player)).Sum(ps => ps.Score));
            }   
         
            Assert.True(gameState.TeamScores.Count(ts => ts.Score >= gameState.GameRules.WinningScore) == 1);

            Assert.True(gameState.IsGameFinished());
            Assert.True(gameState.Rounds.All(r => r.DealtCards.Select(p => p.Hand).All(cards => cards.Distinct().Count() == gameState.GameRules.HandSizeToDeal)));
            Assert.True(gameState.Rounds.All(r => r.Hands.Select(p => p.Hand).All(cards => cards.Distinct().Count() == GameRules.HandSize)));

            Assert.True(
                gameState.TeamScores
                .All(ts =>
                {
                    var showScore = gameState.Rounds.Sum(r => r.ShowScores.Where(ss => ts.Players.Contains(ss.Player)).Sum(pss => pss.ShowScore + (pss.CribScore ?? 0)));
                    var playScores = gameState.Rounds.Sum(r => r.ThePlay.SelectMany(ppi => ppi).Where(ppi => ts.Players.Contains(ppi.Player)).Sum(ppi => ppi.Score));
                    return ts.Score == showScore + playScores;
                }));
        }
    }
}
