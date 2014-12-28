using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.PlayingCards.Order;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Test.System
{
    [TestClass]
    public class GameTestFixture
    {
        private Random _random = new Random(Environment.TickCount);

        [TestMethod]
        public void SmokeTest()
        {
            foreach (var source in Enumerable.Range(0, 10).AsParallel())
            {
                var playerCount = _random.Next() % 2 == 0 ? 4 : 4;
                var game = new CribbageGame(Enumerable.Range(0, playerCount).Select(i => CreateRandomizedPlayer()).ToList(), CreateRandomizedGameRules(playerCount), new Deck(), new ScoreCalculator());
                var result = game.Run();
                Assert.IsTrue(result.IsGameFinished());
            }
        }

        public GameRules CreateRandomizedGameRules(int players)
        {
            return new GameRules(_random.Next() % 2 == 0 ? GameScoreType.Short61 : GameScoreType.Standard121, players);
        }


        public Player CreateRandomizedPlayer()
        {
            return new Player(null, -1, CreateRandomizedPlayStrategy(), CreateRandomizedDecisionStrategy(),
                CreateRandomizeScoreCountStrategy());
        }

        public IPlayStrategy CreateRandomizedPlayStrategy()
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


        public IDecisionStrategy CreateRandomizedDecisionStrategy()
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

        public IScoreCountStrategy CreateRandomizeScoreCountStrategy()
        {
            return new PercentageScoreCountStrategy(_random.Next(80, 100));
        }
    }
}
