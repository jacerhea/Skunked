using System;
using System.Collections.Generic;
using Cribbage.Commands;
using Cribbage.Commands.Arguments;
using Cribbage.Exceptions;
using Cribbage.Rules;
using Cribbage.State;
using Cribbage.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.Test.Commands
{
    [TestClass]
    public class CribbageCommandBaseTestFixture
    {
        private CribGameState _gameState;

        [TestInitialize]
        public void SetUp()
        {

            _gameState = new CribGameState
            {
                GameRules = new CribGameRules(GameScoreType.Standard121, 2),
                Players =
                    new List<Player>
                                         {
                                             new Player("Player1", 1),
                                             new Player("Player2", 2)
                                         },
                OpeningRoundState = new CribOpeningRoundState { },
                PlayerScores = new List<SerializablePlayerScore>
                                         {
                                             new SerializablePlayerScore {Player = 1, Score = 120},
                                             new SerializablePlayerScore {Player = 2, Score = 122}
                                         },
                Rounds = new List<CribRoundState>
                                              {
                                                  new CribRoundState
                                                      {
                                                          PlayerCrib = 1,
                                                          PlayerHand =
                                                              new List<SerializableKeyValuePair<int, List<Card>>>
                                                                  {
                                                                      new SerializableKeyValuePair<int, List<Card>>
                                                                          {
                                                                              Key = 1,
                                                                              Value = new List<Card>
                                                                                          {
                                                                                              new Card(Rank.Six, Suit.Clubs),
                                                                                              new Card(Rank.Seven, Suit.Diamonds),
                                                                                              new Card(Rank.Seven, Suit.Hearts),
                                                                                              new Card(Rank.Eight, Suit.Spades)
                                                                                          }
                                                                          },
                                                                      new SerializableKeyValuePair<int, List<Card>>
                                                                          {
                                                                              Key = 2,
                                                                              Value = new List<Card>
                                                                                          {
                                                                                              new Card(Rank.Four, Suit.Spades),
                                                                                              new Card(Rank.Jack, Suit.Hearts),
                                                                                              new Card(Rank.Six, Suit.Diamonds),
                                                                                              new Card(Rank.Five, Suit.Clubs)
                                                                                          }
                                                                          }
                                                                  },
                                                          PlayersShowedCards = new List<List<PlayerPlayItem>>
                                                                                   {
                                                                                       new List<PlayerPlayItem>()
                                                                                   },
                                                          ThrowCardsIsDone = true,
                                                          PlayCardsIsDone = true,
                                                          StartingCard = new Card(Rank.Eight, Suit.Clubs),
                                                          PlayerShowScores = new List<PlayerScoreShow>
                                                                                 {
                                                                                     new PlayerScoreShow{ ShowScore = 0, HasShowed = false, Player = 1, PlayerCountedShowScore = 0, CribScore = null },
                                                                                     new PlayerScoreShow{ ShowScore = 0, HasShowed = false, Player = 2, PlayerCountedShowScore = 0, CribScore = null }
                                                                                 }
                                                      }
                                              }
            };


        }

        [TestMethod]
        public void Test_Command_Base_Validation_Player_Score_Exception()
        {
            try
            {
                var command = new CribbageCommandBaseTestClass(new CommandArgsBaseMock(_gameState, 1, 0));
                command.Execute();
                Assert.Fail();
            }
            catch (InvalidCribbageOperationException)
            {
                Assert.IsTrue(true);
            }
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
        public CommandArgsBaseMock(CribGameState cribGameState, int playerId, int round)
            : base(cribGameState, playerId, round)
        {
        }
    }
}
