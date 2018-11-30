using System.Collections.Generic;
using System.Linq;
using Moq;
using Skunked.Dealer;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.Dealer
{
    public class StandardHandDealerTestFixture
    {
        [Fact]
        public void Test_Deal()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.GetEnumerator()).Returns(() => new List<Card>
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
            }.GetEnumerator());
            var handFactory = new StandardHandDealer();
            var players = new List<int> {1,2};
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[0], 6).ToDictionary(p => p.PlayerId, p => p.Hand);

            var player1Actual = hands[players[0]];
            Assert.True(new Card(Rank.King, Suit.Clubs).Equals(player1Actual[0]));
            Assert.True(new Card(Rank.Eight, Suit.Hearts).Equals(player1Actual[1]));
            Assert.True(new Card(Rank.Four, Suit.Diamonds).Equals(player1Actual[2]));
            Assert.True(new Card(Rank.Nine, Suit.Diamonds).Equals(player1Actual[3]));
            Assert.True(new Card(Rank.Seven, Suit.Clubs).Equals(player1Actual[4]));
            Assert.True(new Card(Rank.Ten, Suit.Clubs).Equals(player1Actual[5]));
            Assert.Equal(6, player1Actual.Count);

            var player2Actual = hands[players[1]];
            Assert.True(new Card(Rank.Ace, Suit.Diamonds).Equals(player2Actual[0]));
            Assert.True(new Card(Rank.Five, Suit.Spades).Equals(player2Actual[1]));
            Assert.True(new Card(Rank.Jack, Suit.Hearts).Equals(player2Actual[2]));
            Assert.True(new Card(Rank.Queen, Suit.Spades).Equals(player2Actual[3]));
            Assert.True(new Card(Rank.Six, Suit.Hearts).Equals(player2Actual[4]));
            Assert.True(new Card(Rank.Three, Suit.Spades).Equals(player2Actual[5]));
            Assert.Equal(6, player2Actual.Count);
        }

        [Fact]
        public void Test_Deal_StartingWithPlayer2()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.GetEnumerator()).Returns(() => new List<Card>
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
            }.GetEnumerator());
            var handFactory = new StandardHandDealer();
            var players = new List<int> { 1,2};
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[1], 6).ToDictionary(p => p.PlayerId, p => p.Hand);

            var player2Actual = hands[players[1]];
            Assert.True(new Card(Rank.King, Suit.Clubs).Equals(player2Actual[0]));
            Assert.True(new Card(Rank.Eight, Suit.Hearts).Equals(player2Actual[1]));
            Assert.True(new Card(Rank.Four, Suit.Diamonds).Equals(player2Actual[2]));
            Assert.True(new Card(Rank.Nine, Suit.Diamonds).Equals(player2Actual[3]));
            Assert.True(new Card(Rank.Seven, Suit.Clubs).Equals(player2Actual[4]));
            Assert.True(new Card(Rank.Ten, Suit.Clubs).Equals(player2Actual[5]));
            Assert.Equal(6, player2Actual.Count);

            var player1Actual = hands[players[0]];
            Assert.True(new Card(Rank.Ace, Suit.Diamonds).Equals(player1Actual[0]));
            Assert.True(new Card(Rank.Five, Suit.Spades).Equals(player1Actual[1]));
            Assert.True(new Card(Rank.Jack, Suit.Hearts).Equals(player1Actual[2]));
            Assert.True(new Card(Rank.Queen, Suit.Spades).Equals(player1Actual[3]));
            Assert.True(new Card(Rank.Six, Suit.Hearts).Equals(player1Actual[4]));
            Assert.True(new Card(Rank.Three, Suit.Spades).Equals(player1Actual[5]));
            Assert.Equal(6, player1Actual.Count);
        }

        [Fact]
        public void Test_Deal0Cards()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.GetEnumerator()).Returns(() => new List<Card>().GetEnumerator());
            var handFactory = new StandardHandDealer();
            var players = new List<int> { 1,2 };
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[0], 0).ToDictionary(p => p.PlayerId, p => p.Hand);
            Assert.Equal(0, hands[players[0]].Count);
            Assert.Equal(0, hands[players[1]].Count);
        }

        [Fact]
        public void Test_Deal_StartingWithPlayer3()
        {
            var deck = new Mock<Deck>();
            deck.Setup(d => d.GetEnumerator()).Returns(() => new List<Card>
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
                new Card(Rank.Eight, Suit.Diamonds),
                new Card(Rank.Three, Suit.Hearts),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Five, Suit.Clubs),
                new Card(Rank.Three, Suit.Diamonds),
                new Card(Rank.Five, Suit.Hearts),
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Four, Suit.Clubs),
                new Card(Rank.Two, Suit.Diamonds),
            }.GetEnumerator());
            var handFactory = new StandardHandDealer();
            var players = new List<int> { 1,2,3,4};
            const int handSize = 5;
            var hands = handFactory.CreatePlayerHands(deck.Object, players, players[2], handSize).ToDictionary(p => p.PlayerId, p => p.Hand);

            var player1Actual = hands[players[0]].ToList();
            Assert.True(new Card(Rank.Eight, Suit.Hearts).Equals(player1Actual[0]));
            Assert.True(new Card(Rank.Nine, Suit.Diamonds).Equals(player1Actual[1]));
            Assert.True(new Card(Rank.Ten, Suit.Clubs).Equals(player1Actual[2]));
            Assert.True(new Card(Rank.Five, Suit.Clubs).Equals(player1Actual[3]));
            Assert.True(new Card(Rank.Four, Suit.Clubs).Equals(player1Actual[4]));
            Assert.Equal(handSize, player1Actual.Count);


            var player2Actual = hands[players[1]].ToList();
            Assert.True(new Card(Rank.Five, Suit.Spades).Equals(player2Actual[0]));
            Assert.True(new Card(Rank.Queen, Suit.Spades).Equals(player2Actual[1]));
            Assert.True(new Card(Rank.Eight, Suit.Diamonds).Equals(player2Actual[2]));
            Assert.True(new Card(Rank.Three, Suit.Diamonds).Equals(player2Actual[3]));
            Assert.True(new Card(Rank.Two, Suit.Diamonds).Equals(player2Actual[4]));
            Assert.Equal(handSize, player2Actual.Count);

            var player3Actual = hands[players[2]].ToList();
            Assert.True(new Card(Rank.King, Suit.Clubs).Equals(player3Actual[0]));
            Assert.True(new Card(Rank.Four, Suit.Diamonds).Equals(player3Actual[1]));
            Assert.True(new Card(Rank.Seven, Suit.Clubs).Equals(player3Actual[2]));
            Assert.True(new Card(Rank.Three, Suit.Hearts).Equals(player3Actual[3]));
            Assert.True(new Card(Rank.Five, Suit.Hearts).Equals(player3Actual[4]));
            Assert.Equal(handSize, player3Actual.Count);


            var player4Actual = hands[players[3]].ToList();
            Assert.True(new Card(Rank.Ace, Suit.Diamonds).Equals(player4Actual[0]));
            Assert.True(new Card(Rank.Jack, Suit.Hearts).Equals(player4Actual[1]));
            Assert.True(new Card(Rank.Six, Suit.Hearts).Equals(player4Actual[2]));
            Assert.True(new Card(Rank.Three, Suit.Spades).Equals(player4Actual[3]));
            Assert.True(new Card(Rank.Ace, Suit.Spades).Equals(player4Actual[4]));
            Assert.Equal(handSize, player4Actual.Count);
        }
    }
}
