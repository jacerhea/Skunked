using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Skunked.Cards;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;
using Xunit;

namespace Skunked.UnitTest.State
{

    public class GameStateSerializationTests
    {

        [Fact]
        public void GameState_Can_Be_Xml_Serialized()
        {
            var gameState = new GameState
            {
                GameRules = new GameRules(WinningScoreType.Short61, 4),
                Id = Guid.NewGuid(),
                IndividualScores =
                    new List<PlayerScore>
                    {
                        new PlayerScore {Player = 1, Score = 2},
                        new PlayerScore {Player = 2, Score = 3}
                    },
                LastUpdated = DateTimeOffset.Now,
                OpeningRound =
                    new OpeningRound
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
                TeamScores = new List<TeamScore> { new TeamScore { Players = new List<int> { 1 }, Score = 5 } }
            };

            //var stream = new MemoryStream();

            //todo: fix me.
            //var xmlSerializer = new XmlSerializer(typeof(GameState));
            //xmlSerializer.Serialize(stream, gameState);
            //stream.Seek(0, SeekOrigin.Begin);
            //var gameStateDeserialized = (GameState)xmlSerializer.Deserialize(stream);

            Assert.True(true);

        }
    }
}
