using System.Collections.Generic;
using FluentAssertions;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.AI.CardToss
{
    public class DistributionServiceTestFixture
    {
        [Fact]
        public void CalculateDistribution()
        {
            var hand = new List<Card>
            {
                new Card(Rank.Five, Suit.Clubs),
                new Card(Rank.Six, Suit.Clubs),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Five, Suit.Diamonds),
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Queen, Suit.Clubs),
            };

            var distributionService = new DistributionService();
            var distribution = distributionService.CalculateDistribution(hand);
            distribution.Sets.Count.Should().Be(15);
        }

        [Fact]
        public void CalculateDistribution_4Cards()
        {
            var hand = new List<Card>
            {
                new Card(Rank.Five, Suit.Clubs),
                new Card(Rank.Six, Suit.Clubs),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Five, Suit.Diamonds),
            };

            var distributionService = new DistributionService();
            var distribution = distributionService.CalculateDistribution(hand);
            distribution.Sets.Count.Should().Be(6);
        }
    }
}
