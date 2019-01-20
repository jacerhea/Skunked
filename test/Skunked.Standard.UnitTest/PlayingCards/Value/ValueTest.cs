using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.PlayingCards.Value
{
    public class ValueTest
    {
        [Fact]
        public void AceLowFaceTenCardValueStrategyValue()
        {
            var expectedValues = new List<Tuple<Card, int>>
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

            foreach (var testCase in expectedValues)
            {
                int calculatedValue = strategy.ValueOf(testCase.Item1);
                calculatedValue.Should().Be(testCase.Item2);
            }
        }

        [Fact]
        public void ArgumentNull()
        {
            var strategy = new AceLowFaceTenCardValueStrategy();
            Action valueOf = () => strategy.ValueOf(null);
            valueOf.Should().Throw<ArgumentNullException>();
        }
    }
}
