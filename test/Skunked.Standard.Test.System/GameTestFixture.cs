using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.Game;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;
using Xunit;

namespace Skunked.Test.System
{
    public class GameTestFixture
    {
        private readonly Random _random = new Random(Environment.TickCount);

        [Fact]
        public void SmokeTest()
        {
            var results = new ConcurrentBag<Cribbage>();
            Task.WaitAll(Enumerable.Range(0, 10).Select(it =>
            {
                return Task.Run(() =>
                {
                    var playerCount = _random.Next()%2 == 0 ? 2 : 4;
                    var game = new CribbageGameRunner(Enumerable.Range(0, playerCount).Select(i => CreateRandomizedPlayer()).ToList(),CreateRandomizedGameRules(playerCount));
                    var result = game.Run();
                    results.Add(result);
                });
            }).ToArray());

            foreach (var game in results)
            {
                //TestEndGame.Test(game.State);
                //var gameStateBuilder = new GameStateBuilder();
                //var gameState = gameStateBuilder.Build(game.Stream);

                //XmlSerializer ser = new XmlSerializer(typeof(GameState));
                //XmlSerializer eventStreamSerializer = new XmlSerializer(typeof(EventStream));
                //var serializedGame = JsonConvert.SerializeObject(game.Stream.ToList());

                //MemoryStream ms = new MemoryStream();
                //eventStreamSerializer.Serialize(ms, game.Stream);
                //ms.Position = 0;

                //StreamReader r = new StreamReader(ms);
                //string builtGameState = r.ReadToEnd();

                //MemoryStream ms2 = new MemoryStream();
                //ser.Serialize(ms2, game.State);
                //ms2.Position = 0;

                //StreamReader r2 = new StreamReader(ms2);
                //string builtGameState2 = r2.ReadToEnd();
            }
        }

        private GameRules CreateRandomizedGameRules(int players)
        {
            return new GameRules( GameScoreType.Short61, players);
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
