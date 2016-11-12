using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;

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
                    new List<int> {1, 2},
                OpeningRound = new OpeningRound(),
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
                            new PlayerScoreShow {ShowScore = 0, HasShowed = false, Player = 1, PlayerCountedShowScore = 0, CribScore = null},
                            new PlayerScoreShow {ShowScore = 0, HasShowed = false, Player = 2, PlayerCountedShowScore = 0, CribScore = null}
                        }
                    }
                },
                TeamScores =
                    new List<TeamScore>
                    {
                        new TeamScore {Players = new List<int> {1}, Score = 120},
                        new TeamScore {Players = new List<int> {2}, Score = 122}
                    }
            };
        }
    }
}
