using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
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
            var calculator = new ScoreCalculator();
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            Assert.AreEqual(2, cardsToThrow.Count);
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Jack, Suit.Clubs)));
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Five, Suit.Clubs)));
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
            var calculator = new ScoreCalculator();
            var decisionStrategy = new MinAverageDecision(calculator);

            var cardsToThrow = decisionStrategy.DetermineCardsToThrow(hand).ToList();

            Assert.AreEqual(1, cardsToThrow.Count());
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Five, Suit.Clubs)));
        }
    }
}
