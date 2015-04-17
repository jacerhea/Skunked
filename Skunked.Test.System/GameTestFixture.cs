using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTestFixture
    {
        private readonly Random _random = new Random(Environment.TickCount);

        [TestMethod]
        public void SmokeTest()
        {
            var results = new ConcurrentBag<GameState>();
            Task.WaitAll(Enumerable.Range(0, 100).Select(it =>
            {
                return Task.Run(() =>
                {
                    var playerCount = _random.Next()%2 == 0 ? 2 : 4;
                    var game = new CribbageGameRunner(Enumerable.Range(0, playerCount).Select(i => CreateRandomizedPlayer()).ToList(),CreateRandomizedGameRules(playerCount));
                    var result = game.Run();
                    results.Add(result);
                });
            }).ToArray());

            foreach (var gameState in results)
            {
                TestEndGame.Test(gameState);
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
