using FluentAssertions;
using Skunked.Cards;
using Skunked.Cards.Order;
using Xunit;

namespace Skunked.UnitTest.Cards.Order;

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

        var orderStrategy = new RankComparer();

        foreach (var testCase in testCases)
        {
            var sortedByOrderStrategy = testCases.OrderBy(c => c.Item1, RankComparer.Instance).ToList();
            sortedByOrderStrategy.IndexOf(testCase).Should().Be(testCase.Item2);
        }
    }

    [Fact]
    public void Null_Argument1_Will_Throw_ArgumentNullException()
    {
        var orderStrategy = new RankComparer();
        Action order = () => orderStrategy.Compare(null, new Card());
        order.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Null_Argument2_Will_Throw_ArgumentNullException()
    {
        var orderStrategy = new RankComparer();
        Action order = () => orderStrategy.Compare(new Card(), null);
        order.Should().Throw<ArgumentNullException>();
    }
}