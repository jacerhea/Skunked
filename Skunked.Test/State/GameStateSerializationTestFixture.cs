using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.Utility;
using Xunit;

namespace Skunked.Test.State
{

    public class GameStateSerializationTestFixture
    {

        [Fact]
        public void GameStateIsSerializable()
        {
            var gameState = new GameState
            {
                GameRules = new GameRules(GameScoreType.Short61, 4),
                Id = Guid.NewGuid(),
                IndividualScores =
                    new List<PlayerScore>
                    {
                        new PlayerScore {Player = 1, Score = 2},
                        new PlayerScore {Player = 2, Score = 3}
                    },
                LastUpdated = DateTimeOffset.Now,
                OpeningRound =
                    new OpeningRoundState
                    {
                        Complete = true,
                        CutCards =
                            new List<PlayerIdCard>
                            {
                                new PlayerIdCard{Player= 1, Card= new Card()}
                            },
                        WinningPlayerCut = 1,
                        Deck = new Deck().ToList()
                    },
                StartedAt = DateTimeOffset.Now,
                Rounds = new List<RoundState> { new RoundState { Complete = false } },
                PlayerIds = new List<int> { 1, 2 },
                TeamScores = new List<TeamScore> { new TeamScore { Players = new List<int> { 1 }, Score = 5 } },
            };









            var stream = new MemoryStream();




            var xmlSerializer = new XmlSerializer(typeof(GameState));
            xmlSerializer.Serialize(stream, gameState);
            stream.Seek(0, SeekOrigin.Begin);
            var gameStateDeserialized = (GameState)xmlSerializer.Deserialize(stream);

            Assert.True(true);

        }
    }
}
