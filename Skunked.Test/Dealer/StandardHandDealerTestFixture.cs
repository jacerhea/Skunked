using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skunked.Dealer;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.Test.Dealer
{
    [TestClass]
    public class StandardHandDealerTestFixture
    {
        [TestMethod]
        public void Test_Deal()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.Cards).Returns(() => new List<Card>
            {
                new Card(Rank.King, Suit.Clubs),
                new Card(Rank.Ace, Suit.Diamonds),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Four, Suit.Diamonds),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.Nine, Suit.Diamonds),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Six, Suit.Hearts),
                new Card(Rank.Ten, Suit.Clubs),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
            });
            var handFactory = new StandardHandDealer();
            var player1 = new Player();
            var player2 = new Player();
            var players = new List<Player> {player1, player2};
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[0], 6);

            var player1Actual = hands[player1].ToList();
            Assert.IsTrue((new Card(Rank.King, Suit.Clubs)).Equals(player1Actual[0]));
            Assert.IsTrue((new Card(Rank.Eight, Suit.Hearts)).Equals(player1Actual[1]));
            Assert.IsTrue((new Card(Rank.Four, Suit.Diamonds)).Equals(player1Actual[2]));
            Assert.IsTrue((new Card(Rank.Nine, Suit.Diamonds)).Equals(player1Actual[3]));
            Assert.IsTrue((new Card(Rank.Seven, Suit.Clubs)).Equals(player1Actual[4]));
            Assert.IsTrue((new Card(Rank.Ten, Suit.Clubs)).Equals(player1Actual[5]));
            Assert.AreEqual(6, player1Actual.Count);

            var player2Actual = hands[player2].ToList();
            Assert.IsTrue((new Card(Rank.Ace, Suit.Diamonds)).Equals(player2Actual[0]));
            Assert.IsTrue((new Card(Rank.Five, Suit.Spades)).Equals(player2Actual[1]));
            Assert.IsTrue((new Card(Rank.Jack, Suit.Hearts)).Equals(player2Actual[2]));
            Assert.IsTrue((new Card(Rank.Queen, Suit.Spades)).Equals(player2Actual[3]));
            Assert.IsTrue((new Card(Rank.Six, Suit.Hearts)).Equals(player2Actual[4]));
            Assert.IsTrue((new Card(Rank.Three, Suit.Spades)).Equals(player2Actual[5]));
            Assert.AreEqual(6, player2Actual.Count);
        }

        [TestMethod]
        public void Test_Deal_StartingWithPlayer2()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.Cards).Returns(() => new List<Card>
            {
                new Card(Rank.King, Suit.Clubs),
                new Card(Rank.Ace, Suit.Diamonds),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Four, Suit.Diamonds),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.Nine, Suit.Diamonds),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Six, Suit.Hearts),
                new Card(Rank.Ten, Suit.Clubs),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
            });
            var handFactory = new StandardHandDealer();
            var player1 = new Player();
            var player2 = new Player();
            var players = new List<Player> { player1, player2 };
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[1], 6);

            var player2Actual = hands[player2].ToList();
            Assert.IsTrue((new Card(Rank.King, Suit.Clubs)).Equals(player2Actual[0]));
            Assert.IsTrue((new Card(Rank.Eight, Suit.Hearts)).Equals(player2Actual[1]));
            Assert.IsTrue((new Card(Rank.Four, Suit.Diamonds)).Equals(player2Actual[2]));
            Assert.IsTrue((new Card(Rank.Nine, Suit.Diamonds)).Equals(player2Actual[3]));
            Assert.IsTrue((new Card(Rank.Seven, Suit.Clubs)).Equals(player2Actual[4]));
            Assert.IsTrue((new Card(Rank.Ten, Suit.Clubs)).Equals(player2Actual[5]));
            Assert.AreEqual(6, player2Actual.Count);

            var player1Actual = hands[player1].ToList();
            Assert.IsTrue((new Card(Rank.Ace, Suit.Diamonds)).Equals(player1Actual[0]));
            Assert.IsTrue((new Card(Rank.Five, Suit.Spades)).Equals(player1Actual[1]));
            Assert.IsTrue((new Card(Rank.Jack, Suit.Hearts)).Equals(player1Actual[2]));
            Assert.IsTrue((new Card(Rank.Queen, Suit.Spades)).Equals(player1Actual[3]));
            Assert.IsTrue((new Card(Rank.Six, Suit.Hearts)).Equals(player1Actual[4]));
            Assert.IsTrue((new Card(Rank.Three, Suit.Spades)).Equals(player1Actual[5]));
            Assert.AreEqual(6, player1Actual.Count);
        }

        [TestMethod]
        public void Test_Deal0Cards()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.Cards).Returns(() => new List<Card>());
            var handFactory = new StandardHandDealer();
            var player1 = new Player();
            var player2 = new Player();
            var players = new List<Player> { player1, player2 };
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[0], 0);
            Assert.AreEqual(0, hands[player1].Count);
            Assert.AreEqual(0, hands[player2].Count);

        }
    }

}
