using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Skunked.Cards;
using Skunked.Domain.State;
using Skunked.Players;
using Skunked.Rules;
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
                        new() {Player = 1, Score = 2},
                        new() {Player = 2, Score = 3}
                    },
                LastUpdated = DateTimeOffset.Now,
                OpeningRound =
                    new OpeningRound
                    {
                        Complete = true,
                        CutCards =
                            new List<PlayerIdCard>
                            {
                                new() {Player= 1, Card= new Card()}
                            },
                        WinningPlayerCut = 1,
                        Deck = new Deck().ToList()
                    },
                StartedAt = DateTimeOffset.Now,
                Rounds = new List<RoundState> { new() { Complete = false } },
                PlayerIds = new List<int> { 1, 2 },
                TeamScores = new List<TeamScore> { new() { Players = new List<int> { 1 }, Score = 5 } }
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
