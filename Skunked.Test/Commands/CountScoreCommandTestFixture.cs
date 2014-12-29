using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Commands;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Test.Commands
{
    [TestClass]
    public class CountScoreCommandTestFixture
    {
        private GameState _gameState;
        private ScoreCalculator _scoreCalculator;

        [TestInitialize]
        public void SetUp()
        {
            _scoreCalculator = new ScoreCalculator();

            _gameState = new GameState
            {
                Rules = new GameRules(GameScoreType.Standard121, 2),
                Players =
                    new List<Player>
                                         {
                                             new Player("Player1", 1),
                                             new Player("Player2", 2)
                                         },
                OpeningRound = new OpeningRoundState(),
                IndividualScores = new List<PlayerScore>
                                         {
                                             new PlayerScore {Player = 1, Score = 0},
                                             new PlayerScore {Player = 2, Score = 0}
                                         },
                Rounds = new List<RoundState>
                                              {
                                                  new RoundState
                                                      {
                                                          PlayerCrib = 1,
                                                          Hands =
                                                              new List<PlayerIdHand>
                                                                  {
                                                                      new PlayerIdHand(1,new List<Card>
                                                                                          {
                                                                                              new Card(Rank.Six, Suit.Clubs),
                                                                                              new Card(Rank.Seven,Suit.Diamonds),
                                                                                              new Card(Rank.Seven,Suit.Hearts),
                                                                                              new Card(Rank.Eight, Suit.Spades)
                                                                                          }
                                                                          ),
                                                                      new PlayerIdHand( 2,new List<Card>
                                                                                          {
                                                                                              new Card(Rank.Four, Suit.Spades),
                                                                                              new Card(Rank.Jack, Suit.Hearts),
                                                                                              new Card(Rank.Six, Suit.Diamonds),
                                                                                              new Card(Rank.Five, Suit.Clubs)
                                                                                          }
                                                                          )
                                                                  },
                                                          ThePlay = new List<List<PlayerPlayItem>>
                                                                                   {
                                                                                       new List<PlayerPlayItem>()
                                                                                   },
                                                          ThrowCardsComplete = true,
                                                          PlayedCardsComplete = true,
                                                          Starter = new Card(Rank.Eight, Suit.Clubs),
                                                          ShowScores = new List<PlayerScoreShow>
                                                                                 {
                                                                                     new PlayerScoreShow{ ShowScore = 0, HasShowed = false, Player = 1, PlayerCountedShowScore = 0, CribScore = null },
                                                                                     new PlayerScoreShow{ ShowScore = 0, HasShowed = false, Player = 2, PlayerCountedShowScore = 0, CribScore = null }
                                                                                 },
                                                          Round = 1
                                                      }
                                              },
                TeamScores = new List<TeamScore> { new TeamScore { Players = new List<int> { 1 } }, new TeamScore { Players = new List<int> { 2} } }
            };
        }

        [TestMethod]
        public void Test_Current_Round_Is_Done_Throws_Exception()
        {
            _gameState.GetCurrentRound().Complete = true;
            var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 10));

            try
            {
                command.Execute();
                Assert.Fail();
            }
            catch (InvalidCribbageOperationException exception)
            {
                Assert.IsTrue(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
            }
        }

        [TestMethod]
        public void Test_Current_Round_Throw_Cards_Is_Not_Done_Throws_Exception()
        {
            _gameState.GetCurrentRound().ThrowCardsComplete = false;
            var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 10));

            try
            {
                command.Execute();
                Assert.Fail();
            }
            catch (InvalidCribbageOperationException exception)
            {
                Assert.IsTrue(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
            }
        }

        [TestMethod]
        public void Test_Current_Round_Play_Cards_Is_Not_Done_Throws_Exception()
        {
            _gameState.GetCurrentRound().PlayedCardsComplete = false;
            var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 10));

            try
            {
                command.Execute();
                Assert.Fail();
            }
            catch (InvalidCribbageOperationException exception)
            {
                Assert.IsTrue(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Invalid_Player_Counted_Score()
        {
            new CountHandScoreCommand(new CountHandScoreArgs(_gameState, -1, 1, _scoreCalculator, 24));
        }

        [TestMethod]
        public void Test_Count_Player1_Score()
        {
            const int playerId = 1;
            _gameState.GetCurrentRound().PlayerCrib = 2;

            var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 1));
            command.Execute();

            Assert.AreEqual(1, _gameState.IndividualScores.Single(ps => ps.Player == playerId).Score);
        }

        [TestMethod]
        public void Test_Count_Not_Players_Turn_To_Count_Exception()
        {
            try
            {
                new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 24)).Execute();
                Assert.Fail();
            }
            catch (InvalidCribbageOperationException exception)
            {
                Assert.IsTrue(exception.Operation == InvalidCribbageOperations.NotPlayersTurn);
            }
        }

        [TestMethod]
        public void Test_Count_Player2_Score()
        {
            const int playerId = 2;
            _gameState.GetCurrentRound().PlayerCrib = 2;

            var command1 = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, _scoreCalculator, 24));
            command1.Execute();

            var command2 = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, playerId, 1, _scoreCalculator, 1));
            command2.Execute();

            Assert.AreEqual(1, _gameState.IndividualScores.Single(ps => ps.Player == playerId).Score);
        }
    }
}
