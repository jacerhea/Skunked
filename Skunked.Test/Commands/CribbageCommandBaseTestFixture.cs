using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Commands;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Test.Commands
{
    [TestClass]
    public class CribbageCommandBaseTestFixture
    {
        private GameState _gameState;

        [TestInitialize]
        public void SetUp()
        {

            _gameState = new GameState
            {
                GameRules = new GameRules(GameScoreType.Standard121, 2),
                PlayerIds =
                    new List<int>{1,2},
                OpeningRound = new OpeningRoundState { },
                IndividualScores = new List<PlayerScore>
                                         {
                                             new PlayerScore {Player = 1, Score = 120},
                                             new PlayerScore {Player = 2, Score = 122}
                                         },
                Rounds = new List<RoundState>
                                              {
                                                  new RoundState
                                                      {
                                                          PlayerCrib = 1,
                                                          Hands =
                                                              new List<PlayerIdHand>
                                                                  {
                                                                      new PlayerIdHand(1, new List<Card>
                                                                                          {
                                                                                              new Card(Rank.Six, Suit.Clubs),
                                                                                              new Card(Rank.Seven, Suit.Diamonds),
                                                                                              new Card(Rank.Seven, Suit.Hearts),
                                                                                              new Card(Rank.Eight, Suit.Spades)
                                                                                          }
                                                                          ),
                                                                      new PlayerIdHand
                                                                          (2, new List<Card>
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
                                                                                 }
                                                      }
                                              },
                TeamScores = new List<TeamScore> { new TeamScore { Players = new List<int> { 1 }, Score = 120}, new TeamScore { Players = new List<int> { 2}, Score = 122} }
            };


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCribbageOperationException))]
        public void Test_Command_Base_Validation_Player_Score_Exception()
        {
            var command = new CribbageCommandBaseTestClass(new CommandArgsBaseMock(new EventStream(new List<IEventListener>()),  _gameState, 1, 0));
            command.Execute();
            Assert.Fail();
        }
    }

    public class CribbageCommandBaseTestClass : CribbageCommandBase, ICommand
    {
        public CribbageCommandBaseTestClass(CommandArgsBase args)
            : base(args)
        {

        }

        protected override void ValidateState()
        {
        }

        public void Execute()
        {
            ValidateStateBase();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }

    public class CommandArgsBaseMock : CommandArgsBase
    {
        public CommandArgsBaseMock(EventStream eventStream, GameState gameState, int playerId, int round)
            : base(gameState, playerId, round)
        {
        }
    }
}
