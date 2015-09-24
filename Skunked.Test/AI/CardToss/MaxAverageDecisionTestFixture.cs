using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;

namespace Skunked.Test.AI.CardToss
{
    [TestClass]
    public class MaxAverageDecisionTestFixture
    {
        [TestMethod]
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

            Assert.AreEqual(2, cardsToThrow.Count);
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Two, Suit.Spades)));
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Nine, Suit.Hearts)));
        }

        [TestMethod]
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

            Assert.AreEqual(1, cardsToThrow.Count);
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.Nine, Suit.Hearts)));
        }

        [TestMethod]
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

            Assert.AreEqual(2, cardsToThrow.Count);
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.King, Suit.Clubs)));
            Assert.IsTrue(cardsToThrow.Contains(new Card(Rank.King, Suit.Spades)));
        }
    }
}
