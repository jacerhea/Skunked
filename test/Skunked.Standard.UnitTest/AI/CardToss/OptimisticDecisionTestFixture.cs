using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.AI.CardToss
{
    public class AITest
    {
        [Fact]
        public void OptimisticDecisionTest()
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
            var decisionStrategy = new OptimisticDecision();

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            cardsToThrow.Count.Should().Be(2);
            cardsToThrow.Should().Contain(new Card(Rank.Two, Suit.Spades))
                .And.Contain(new Card(Rank.Nine, Suit.Hearts));
        }

        [Fact]
        public void OptimisticDecisionTest_FiveCards()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Clubs),
                               new Card(Rank.Jack, Suit.Clubs),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Hearts),
                           };
            var decisionStrategy = new OptimisticDecision();

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            cardsToThrow.Count.Should().Be(1);
            cardsToThrow.Should().Contain(new Card(Rank.Nine, Suit.Hearts));
        }
    }
}
