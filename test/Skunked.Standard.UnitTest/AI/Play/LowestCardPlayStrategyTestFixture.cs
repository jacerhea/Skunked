using System.Collections.Generic;
using FluentAssertions;
using Skunked.AI.Play;
using Skunked.PlayingCards;
using Skunked.Rules;
using Xunit;

namespace Skunked.Test.AI.Play
{
    public class LowestCardPlayStrategyTestFixture
    {
        private readonly LowestCardPlayStrategy _lowestCardPlayStrategy = new LowestCardPlayStrategy();

        [Fact]
        public void Test_LowerCardIsPlayed()
        {
            var gameRules = new GameRules();
            var pile = new List<Card>();
            var handLeft = new List<Card>
            {
                new Card(Rank.Ace, Suit.Clubs),
                new Card(Rank.King, Suit.Diamonds),
                new Card(Rank.Two, Suit.Clubs),
                new Card(Rank.Queen, Suit.Spades)
            };

            var thrown = _lowestCardPlayStrategy.DetermineCardToThrow(gameRules, pile, handLeft);
            thrown.Should().Be(new Card(Rank.Ace, Suit.Clubs));
        }

        [Fact]
        public void Test_LowerCardIsPlayed_EqualValue()
        {
            var gameRules = new GameRules();
            var pile = new List<Card>();
            var handLeft = new List<Card>
            {
                new Card(Rank.Two, Suit.Clubs),
                new Card(Rank.Two, Suit.Diamonds),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Two, Suit.Spades)
            };

            var thrown = _lowestCardPlayStrategy.DetermineCardToThrow(gameRules, pile, handLeft);
            thrown.Should().Be(new Card(Rank.Two, Suit.Clubs));
        }
    }
}
