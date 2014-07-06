using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.PlayingCards;
using Skunked.PlayingCards.Order;

namespace Skunked.Test.PlayingCards.Order
{
    [TestClass]
    public class OrderTest
    {

        [TestMethod]
        public void Cards()
        {
            var testCases = new List<Tuple<Card, int>>
            {
                Tuple.Create(new Card(Rank.King), 4),
                Tuple.Create(new Card(Rank.Five), 1),
                Tuple.Create(new Card(Rank.Ace), 0),
                Tuple.Create(new Card(Rank.Nine), 2),
                Tuple.Create(new Card(Rank.Jack), 3),
            };

            var orderStrategy = new StandardOrder();

            foreach (var testcase in testCases)
            {
                var sortedByOrderStrategy = testCases.OrderBy(c => orderStrategy.Order(c.Item1)).ToList();
                Assert.AreEqual(testcase.Item2, sortedByOrderStrategy.IndexOf(testcase));
            }
        }

        [ExpectedException(typeof (ArgumentNullException))]
        [TestMethod]
        public void ArgumentNull()
        {
            var orderStrategy = new StandardOrder();
            orderStrategy.Order(null);
        }
    }
}