﻿using Skunked.Cards;
using Skunked.Domain.State;
using Skunked.Rules;
using Skunked.Score;

namespace Skunked.UnitTest.Commands;

public class CountScoreCommandTests
{
    private GameState _gameState;
    private ScoreCalculator _scoreCalculator;

    public CountScoreCommandTests()
    {
        _scoreCalculator = new ScoreCalculator();

        _gameState = new GameState
        {
            GameRules = new GameRules(WinningScoreType.Standard121),
            PlayerIds = [1, 2],
            OpeningRound = new OpeningRound(),
            IndividualScores =
            [
                new() { Player = 1, Score = 0 },
                new() { Player = 2, Score = 0 }
            ],
            Rounds =
            [
                new()
                {
                    PlayerCrib = 1,
                    Hands =
                    [
                        new(1, [
                                new(Rank.Six, Suit.Clubs),
                                new(Rank.Seven, Suit.Diamonds),
                                new(Rank.Seven, Suit.Hearts),
                                new(Rank.Eight, Suit.Spades)
                            ]
                        ),

                        new(2, [
                                new(Rank.Four, Suit.Spades),
                                new(Rank.Jack, Suit.Hearts),
                                new(Rank.Six, Suit.Diamonds),
                                new(Rank.Five, Suit.Clubs)
                            ]
                        )
                    ],
                    ThePlay = [new()],
                    ThrowCardsComplete = true,
                    PlayedCardsComplete = true,
                    Starter = new Card(Rank.Eight, Suit.Clubs),
                    ShowScores =
                    [
                        new()
                        {
                            ShowScore = 0, HasShowed = false, Player = 1, PlayerCountedShowScore = 0, CribScore = null
                        },

                        new()
                        {
                            ShowScore = 0, HasShowed = false, Player = 2, PlayerCountedShowScore = 0, CribScore = null
                        }
                    ],
                    Round = 1
                }
            ],
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }]
        };
    }

    //[Fact]
    //public void Test_Current_Round_Is_Done_Throws_Exception()
    //{
    //    _gameState.GetCurrentRound().Complete = true;
    //    var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 10, _scoreCalculator));

    //    try
    //    {
    //        command.Execute();
    //        Assert.Fail();
    //    }
    //    catch (InvalidCribbageOperationException exception)
    //    {
    //        Assert.True(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
    //    }
    //}

    //[Fact]
    //public void Test_Current_Round_Throw_Cards_Is_Not_Done_Throws_Exception()
    //{
    //    _gameState.GetCurrentRound().ThrowCardsComplete = false;
    //    var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 10, _scoreCalculator));

    //    try
    //    {
    //        command.Execute();
    //        Assert.Fail();
    //    }
    //    catch (InvalidCribbageOperationException exception)
    //    {
    //        Assert.True(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
    //    }
    //}

    //[Fact]
    //public void Test_Current_Round_Play_Cards_Is_Not_Done_Throws_Exception()
    //{
    //    _gameState.GetCurrentRound().PlayedCardsComplete = false;
    //    var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 10, _scoreCalculator));

    //    try
    //    {
    //        command.Execute();
    //        Assert.Fail();
    //    }
    //    catch (InvalidCribbageOperationException exception)
    //    {
    //        Assert.True(exception.Operation == InvalidCribbageOperations.InvalidStateForCount);
    //    }
    //}

    //[Fact]
    //[ExpectedException(typeof(ArgumentOutOfRangeException))]
    //public void Test_Invalid_Player_Counted_Score()
    //{
    //    new CountHandScoreCommand(new CountHandScoreArgs(_gameState, -1, 1, 24, _scoreCalculator));
    //}

    //[Fact]
    //public void Test_Count_Player1_Score()
    //{
    //    const int playerId = 1;
    //    _gameState.GetCurrentRound().PlayerCrib = 2;

    //    var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 1, _scoreCalculator));
    //    command.Execute();

    //    Assert.Equal(1, _gameState.IndividualScores.Single(ps => ps.TestPlayer == playerId).Score);
    //}

    //[Fact]
    //public void Test_Count_Not_Players_Turn_To_Count_Exception()
    //{
    //    try
    //    {
    //        new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 24, _scoreCalculator)).Execute();
    //        Assert.Fail();
    //    }
    //    catch (InvalidCribbageOperationException exception)
    //    {
    //        Assert.True(exception.Operation == InvalidCribbageOperations.NotPlayersTurn);
    //    }
    //}

    //[Fact]
    //public void Test_Count_Player2_Score()
    //{
    //    const int playerId = 2;
    //    _gameState.GetCurrentRound().PlayerCrib = 2;

    //    var command1 = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, 1, 1, 24, _scoreCalculator));
    //    command1.Execute();

    //    var command2 = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, playerId, 1, 1, _scoreCalculator));
    //    command2.Execute();

    //    Assert.Equal(1, _gameState.IndividualScores.Single(ps => ps.TestPlayer == playerId).Score);
    //}
}