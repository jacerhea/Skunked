﻿using Skunked.Cards;
using Skunked.Domain.State;
using Skunked.Rules;
using Xunit;

namespace Skunked.UnitTest.Commands;

public class PlayCardCommandTests
{
    private GameState CreateGameState()
    {
        return new()
        {
            Rounds =
            [
                new()
                {
                    ThePlay =
                    [
                        new()
                        {
                            new PlayItem
                                { Card = new Card(Rank.Jack, Suit.Diamonds), Player = 1, NextPlayer = 2, Score = 0 },
                            new PlayItem
                                { Card = new Card(Rank.Queen, Suit.Clubs), Player = 2, NextPlayer = 1, Score = 0 },
                            new PlayItem
                                { Card = new Card(Rank.Nine, Suit.Spades), Player = 1, NextPlayer = 2, Score = 0 }
                        }
                    ],
                    Round = 1,
                    ThrowCardsComplete = true,
                    PlayedCardsComplete = false,
                    Hands =
                    [
                        new(1,
                        [
                            new(Rank.Jack, Suit.Diamonds), new(Rank.Nine, Suit.Spades), new(Rank.Seven),
                            new(Rank.Four)
                        ]),

                        new(2,
                            [new(Rank.Queen, Suit.Clubs), new(Rank.Ace, Suit.Hearts), new(Rank.Nine), new(Rank.Eight)])
                    ]
                }
            ],
            GameRules = new GameRules(),
            PlayerIds = [1, 2],
            IndividualScores = [new() { Player = 1 }, new() { Player = 2 }],
            TeamScores = [new() { Players = [1] }, new() { Players = [2] }]
        };
    }



    [Fact(Skip = "")]
    public void TestGoIsCounted()
    {
        //var gameState = CreateGameState();
        //var command = new PlayCardCommand(new PlayCardArgs(gameState, 2, 1, new Card(Rank.Ace, Suit.Hearts), new ScoreCalculator()));
        //var playerScorePrior = gameState.IndividualScores.Single(ps => ps.TestPlayer == 2).Score;
        //command.Execute();
        //var currentRound = gameState.GetCurrentRound();
        //Assert.Equal(currentRound.ThePlay.First().Last().Score, 1);
        //Assert.Equal(playerScorePrior + 1, gameState.IndividualScores.Single(ps => ps.TestPlayer == 2).Score);
    }
}