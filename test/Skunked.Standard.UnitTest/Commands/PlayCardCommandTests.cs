using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;
using Skunked.Rules;
using Skunked.State;
using Xunit;

namespace Skunked.UnitTest.Commands
{
    public class PlayCardCommandTests
    {
        private GameState CreateGameState()
        {
            return new GameState
            {
                Rounds = new List<RoundState>
                {
                    new RoundState
                    {
                        ThePlay = new List<List<PlayItem>>
                        {
                            new List<PlayItem>{new PlayItem{Card = new Card(Rank.Jack, Suit.Diamonds), Player = 1, NextPlayer = 2, Score = 0},
                                new PlayItem{Card = new Card(Rank.Queen, Suit.Clubs), Player = 2, NextPlayer = 1, Score = 0},
                                new PlayItem{Card = new Card(Rank.Nine, Suit.Spades), Player = 1, NextPlayer = 2, Score = 0}}
                        },
                        Round = 1,
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = false,
                        Hands = new List<PlayerHand>
                        {
                            new PlayerHand(1, new List<Card>{new Card(Rank.Jack, Suit.Diamonds), new Card(Rank.Nine, Suit.Spades), new Card(Rank.Seven), new Card(Rank.Four)}), 
                            new PlayerHand(2, new List<Card>{new Card(Rank.Queen, Suit.Clubs), new Card(Rank.Ace, Suit.Hearts), new Card(Rank.Nine), new Card(Rank.Eight)})
                        }
                    }
                },
                GameRules = new GameRules(),
                PlayerIds = new List<int> { 1, 2 },
                IndividualScores = new List<PlayerScore> { new PlayerScore { Player = 1 }, new PlayerScore { Player = 2 } },
                TeamScores = new List<TeamScore> { new TeamScore { Players = new List<int> { 1 } }, new TeamScore { Players = new List<int> { 2 } } }
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
}
