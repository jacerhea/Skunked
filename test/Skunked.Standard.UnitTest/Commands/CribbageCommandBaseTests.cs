using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;

namespace Skunked.UnitTest.Commands
{
    public class CribbageCommandBaseTests
    {
        private GameState _gameState;

        public CribbageCommandBaseTests()
        {

            _gameState = new GameState
            {
                GameRules = new GameRules(WinningScoreType.Standard121, 2),
                PlayerIds =
                    new List<int> {1, 2},
                OpeningRound = new OpeningRound(),
                IndividualScores = new List<PlayerScore>
                {
                    new() {Player = 1, Score = 120},
                    new() {Player = 2, Score = 122}
                },
                Rounds = new List<RoundState>
                {
                    new()
                    {
                        PlayerCrib = 1,
                        Hands =
                            new List<PlayerHand>
                            {
                                new(1, new List<Card>
                                {
                                    new(Rank.Six, Suit.Clubs),
                                    new(Rank.Seven, Suit.Diamonds),
                                    new(Rank.Seven, Suit.Hearts),
                                    new(Rank.Eight, Suit.Spades)
                                }
                                    ),
                                new(2, new List<Card>
                                    {
                                        new(Rank.Four, Suit.Spades),
                                        new(Rank.Jack, Suit.Hearts),
                                        new(Rank.Six, Suit.Diamonds),
                                        new(Rank.Five, Suit.Clubs)
                                    }
                                    )
                            },
                        ThePlay = new List<List<PlayItem>>
                        {
                            new()
                        },
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = true,
                        Starter = new Card(Rank.Eight, Suit.Clubs),
                        ShowScores = new List<PlayerScoreShow>
                        {
                            new() {ShowScore = 0, HasShowed = false, Player = 1, PlayerCountedShowScore = 0, CribScore = null},
                            new() {ShowScore = 0, HasShowed = false, Player = 2, PlayerCountedShowScore = 0, CribScore = null}
                        }
                    }
                },
                TeamScores =
                    new List<TeamScore>
                    {
                        new() {Players = new List<int> {1}, Score = 120},
                        new() {Players = new List<int> {2}, Score = 122}
                    }
            };
        }
    }
}
