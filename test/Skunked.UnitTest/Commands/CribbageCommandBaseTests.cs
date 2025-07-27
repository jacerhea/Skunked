using Skunked.Cards;
using Skunked.Domain.State;
using Skunked.Rules;

namespace Skunked.UnitTest.Commands;

public class CribbageCommandBaseTests
{
    private GameState _gameState;

    public CribbageCommandBaseTests()
    {

        _gameState = new GameState
        {
            GameRules = new GameRules(WinningScoreType.Standard121),
            PlayerIds =
                [1, 2],
            OpeningRound = new OpeningRound(),
            IndividualScores =
            [
                new() { Player = 1, Score = 120 },
                new() { Player = 2, Score = 122 }
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
                    ]
                }
            ],
            TeamScores =
            [
                new() { Players = [1], Score = 120 },
                new() { Players = [2], Score = 122 }
            ]
        };
    }
}