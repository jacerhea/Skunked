using System;
using System.Collections.Generic;
using Cribbage.PlayingCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.PlayingCards;
using Skunked.PlayingCards.Value;

namespace Skunked.Test.PlayingCards.Value
{
    [TestClass]
    public class ValueTest
    {
        [TestMethod]
        public void AceLowFaceTenCardValueStrategyValue()
        {
            var testCases = new List<Tuple<Card, int>>
            {
                Tuple.Create(new Card(Rank.Ace), 1),
                Tuple.Create(new Card(Rank.Two), 2),
                Tuple.Create(new Card(Rank.Three), 3),
                Tuple.Create(new Card(Rank.Four), 4),
                Tuple.Create(new Card(Rank.Five), 5),
                Tuple.Create(new Card(Rank.Six), 6),
                Tuple.Create(new Card(Rank.Seven), 7),
                Tuple.Create(new Card(Rank.Eight), 8),
                Tuple.Create(new Card(Rank.Nine), 9),
                Tuple.Create(new Card(Rank.Ten), 10),
                Tuple.Create(new Card(Rank.Jack), 10),
                Tuple.Create(new Card(Rank.Queen), 10),
                Tuple.Create(new Card(Rank.King), 10),
            };

            var strategy = new AceLowFaceTenCardValueStrategy();

            foreach (var testCase in testCases)
            {
                int calculatedValue = strategy.ValueOf(testCase.Item1);
                Assert.AreEqual(testCase.Item2, calculatedValue);
            }
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ArgumentNull()
        {
            var strategy = new AceLowFaceTenCardValueStrategy();
            strategy.ValueOf(null);
        }
    }
}
