using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.Commands;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.Test.Commands
{
    [TestClass]
    public class PlayCardCommandTestFixture
    {
        private GameState CreateGameState()
        {
            return new GameState
            {
                Rounds = new List<RoundState>
                {
                    new RoundState
                    {
                        ThePlay = new List<List<PlayerPlayItem>>
                        {
                            new List<PlayerPlayItem>{new PlayerPlayItem{Card = new Card(Rank.Jack, Suit.Diamonds), Player = 1, NextPlayer = 2, Score = 0},
                                new PlayerPlayItem{Card = new Card(Rank.Queen, Suit.Clubs), Player = 2, NextPlayer = 1, Score = 0},
                                new PlayerPlayItem{Card = new Card(Rank.Nine, Suit.Spades), Player = 1, NextPlayer = 2, Score = 0}}
                        },
                        Round = 1,
                        ThrowCardsComplete = true,
                        PlayedCardsComplete = false,
                        Hands = new List<PlayerIdHand>
                        {
                            new PlayerIdHand(1, new List<Card>{new Card(Rank.Jack, Suit.Diamonds), new Card(Rank.Nine, Suit.Spades), new Card(Rank.Seven), new Card(Rank.Four)}), 
                            new PlayerIdHand(2, new List<Card>{new Card(Rank.Queen, Suit.Clubs), new Card(Rank.Ace, Suit.Hearts), new Card(Rank.Nine), new Card(Rank.Eight)})
                        },
                    }
                },
                GameRules = new GameRules(),
                Players = new List<Player> { new Player(id: 1), new Player(id: 2) },
                IndividualScores = new List<PlayerScore> { new PlayerScore { Player = 1 }, new PlayerScore { Player = 2 } },
                TeamScores = new List<TeamScore> { new TeamScore{Players = new List<int>{1}}, new TeamScore { Players = new List<int>{2}} }
            };
        }



        [TestMethod]
        public void TestGoIsCounted()
        {
            var gameState = CreateGameState();
            var command = new PlayCardCommand(new PlayCardArgs(gameState, 2, 1, new Card(Rank.Ace, Suit.Hearts), new ScoreCalculator()));
            var playerScorePrior = gameState.IndividualScores.Single(ps => ps.Player == 2).Score;
            command.Execute();
            var currentRound = gameState.GetCurrentRound();
            Assert.AreEqual(currentRound.ThePlay.First().Last().Score, 1);
            Assert.AreEqual(playerScorePrior + 1, gameState.IndividualScores.Single(ps => ps.Player == 2).Score);
        }
    }
}
