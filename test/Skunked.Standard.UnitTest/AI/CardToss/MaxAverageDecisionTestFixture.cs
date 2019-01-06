using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.AI.CardToss
{
    public class MaxAverageDecisionTestFixture
    {
        [Fact]
        public void MaxAverageDecisionTest_Six_Cards()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Clubs),
                               new Card(Rank.Jack, Suit.Clubs),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Hearts),
                               new Card(Rank.Two, Suit.Spades),
                           };
            var decisionStrategy = new MaxAverageDecision();

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            cardsToThrow.Count.Should().Be(2);
            cardsToThrow.Should().Contain(new Card(Rank.Two, Suit.Spades));
            cardsToThrow.Should().Contain(new Card(Rank.Nine, Suit.Hearts));
        }

        [Fact]
        public void MaxAverageDecisionTest_FiveCards()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Clubs),
                               new Card(Rank.Jack, Suit.Clubs),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Hearts),
                           };
            var decisionStrategy = new MaxAverageDecision();

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            Assert.Equal(1, cardsToThrow.Count);
            Assert.True(cardsToThrow.Contains(new Card(Rank.Nine, Suit.Hearts)));
        }

        [Fact]
        public void MaxAverageDecisionTest_Test_Six_With_Prior_Failure()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.King, Suit.Spades),
                               new Card(Rank.Seven, Suit.Spades),
                               new Card(Rank.Eight, Suit.Clubs),
                               new Card(Rank.Eight, Suit.Spades),
                           };
            var decisionStrategy = new MaxAverageDecision();

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            Assert.Equal(2, cardsToThrow.Count);
            Assert.True(cardsToThrow.Contains(new Card(Rank.King, Suit.Clubs)));
            Assert.True(cardsToThrow.Contains(new Card(Rank.King, Suit.Spades)));
        }
    }
}
