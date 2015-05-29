using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;

namespace Skunked.Test.AI.CardToss
{
    [TestClass]
    public class DistributionServiceTestFixture
    {
        [TestMethod]
        public void sdafdsa()
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
            Assert.AreEqual(15, distribution.Sets.Count);
        }
    }
}
