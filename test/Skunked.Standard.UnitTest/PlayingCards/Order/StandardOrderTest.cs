using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.PlayingCards;
using Skunked.PlayingCards.Order;
using Xunit;

namespace Skunked.Standard.UnitTest.PlayingCards.Order
{
    public class OrderTest
    {

        [Fact]
        public void Five_Cards_Will_Be_Sorted_Correctly()
        {
            var testCases = new List<Tuple<Card, int>>
            {
                Tuple.Create(new Card(Rank.King), 4),
                Tuple.Create(new Card(Rank.Five), 1),
                Tuple.Create(new Card(Rank.Ace), 0),
                Tuple.Create(new Card(Rank.Nine), 2),
                Tuple.Create(new Card(Rank.Jack), 3)
            };

            var orderStrategy = new StandardOrder();

            foreach (var testCase in testCases)
            {
                var sortedByOrderStrategy = testCases.OrderBy(c => orderStrategy.Order(c.Item1)).ToList();
                sortedByOrderStrategy.IndexOf(testCase).Should().Be(testCase.Item2);
            }
        }

        [Fact]
        public void Null_Argument_Will_Throw_ArgumentNullException()
        {
            var orderStrategy = new StandardOrder();
            Action order = () => orderStrategy.Order(null);
            order.Should().Throw<ArgumentNullException>();
        }
    }
}