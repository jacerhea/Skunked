using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Skunked.PlayingCards.Order;
using Skunked.PlayingCards.Value;
using Skunked.Score;

namespace Skunked.Test.AI.CardToss
{
    [TestClass]
    public class MinAverageDecisionTestFixture
    {
        [TestMethod]
        public void MinAverageDecisionTest_Six_Cards()
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
            var calculator = new ScoreCalculator(new AceLowFaceTenCardValueStrategy(), new StandardOrder());
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand);

            Assert.AreEqual(2, cardsToThrow.Count());
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Two, Suit.Spades)));
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Nine, Suit.Hearts)));
        }

        [TestMethod]
        public void MinAverageDecisionTest_FiveCards()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Clubs),
                               new Card(Rank.Jack, Suit.Clubs),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Hearts),
                           };
            var calculator = new ScoreCalculator(new AceLowFaceTenCardValueStrategy(), new StandardOrder());
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand);

            Assert.AreEqual(1, cardsToThrow.Count());
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Nine, Suit.Hearts)));
        }
    }
}
