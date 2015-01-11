using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTestFixture
    {
        private readonly Random _random = new Random(Environment.TickCount);

        [TestMethod]
        public async void SmokeTest()
        {
            var results = new ConcurrentBag<GameState>();
            Task.WaitAll(Enumerable.Range(0, 100).Select(it =>
            {
                return Task.Run(() =>
                {
                    var playerCount = _random.Next()%2 == 0 ? 2 : 4;
                    var game = new CribbageGame(Enumerable.Range(0, playerCount).Select(i => CreateRandomizedPlayer()).ToList(),CreateRandomizedGameRules(playerCount));
                    var result = game.Run();
                    results.Add(result);
                });
            }).ToArray());

            Assert.IsTrue(results.All(r => r.IsGameFinished()));
            foreach (var gameState in results)
            {
                foreach (var teamScore in gameState.TeamScores)
                {
                    Assert.AreEqual(teamScore.Score, gameState.IndividualScores.Where(s => teamScore.Players.Contains(s.Player)).Sum(ps => ps.Score));
                }
            }
        }

        private GameRules CreateRandomizedGameRules(int players)
        {
            return new GameRules(_random.Next() % 2 == 0 ? GameScoreType.Short61 : GameScoreType.Standard121, players);
        }


        private Player CreateRandomizedPlayer()
        {
            return new Player(null, -1, CreateRandomizedPlayStrategy(), CreateRandomizedDecisionStrategy(),
                CreateRandomizeScoreCountStrategy());
        }

        private IPlayStrategy CreateRandomizedPlayStrategy()
        {
            var mod = _random.Next() % 4;
            if (mod == 0)
            {
                return new LowestCardPlayStrategy();
            }
            if (mod == 1)
            {
                return new MaxPlayStrategy();
            }
            if (mod == 2)
            {
                return new MinPlayStrategy();
            }
            if (mod == 3)
            {
                return new RandomPlayStrategy();
            }
            throw new Exception();
        }


        private IDecisionStrategy CreateRandomizedDecisionStrategy()
        {
            var mod = _random.Next() % 4;
            if (mod == 0)
            {
                return new MaxAverageDecision();
            }
            if (mod == 1)
            {
                return new MinAverageDecision();
            }
            if (mod == 2)
            {
                return new OptimisticDecision();
            }
            if (mod == 3)
            {
                return new RandomDecision();
            }
            throw new Exception();
        }

        private IScoreCountStrategy CreateRandomizeScoreCountStrategy()
        {
            return new PercentageScoreCountStrategy(_random.Next(80, 100));
        }
    }
}
