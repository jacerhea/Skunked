using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.PlayingCards.Order
{
    public class OrderTest
    {

        [Fact]
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
                Assert.Equal(testcase.Item2, sortedByOrderStrategy.IndexOf(testcase));
            }
        }

        [Fact]
        public void ArgumentNull()
        {
            var orderStrategy = new StandardOrder();
            Action order = () => orderStrategy.Order(null);
            order.ShouldThrow<ArgumentNullException>();
        }
    }
}