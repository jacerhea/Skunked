using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Skunked.Score;
using Xunit;

namespace Skunked.Test.AI.CardToss
{
    public class MinAverageDecisionTestFixture
    {
        [Fact]
        public void MinAverageDecision_Throws_Two_Most_Useful_Cards()
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
            var calculator = new ScoreCalculator();
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            //assertions
            cardsToThrow.Should().HaveCount(2);
            cardsToThrow.Should().Contain(new Card(Rank.Jack, Suit.Clubs));
            cardsToThrow.Should().Contain(new Card(Rank.Five, Suit.Clubs));
        }

        [Fact]
        public void MinAverageDecision_Throws_Most_Valuable_Card()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Clubs),
                               new Card(Rank.Jack, Suit.Clubs),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Hearts),
                           };
            var calculator = new ScoreCalculator();
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            cardsToThrow.Should().HaveCount(1);
            cardsToThrow.Should().Contain(new Card(Rank.Five, Suit.Clubs));
        }
    }
}
