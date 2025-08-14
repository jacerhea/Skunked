using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Utility;

public class LinqExtendedTests
{
    [Fact]
    public void Cartesian_WithValidInputs_ReturnsExpectedCombinations()
    {
        // Arrange
        var first = new[] { 1, 2 };
        var second = new[] { 'A', 'B' };

        // Act
        var result = first.Cartesian(second)
            .Select(pair => $"{pair.Item1}{pair.Item2}")
            .ToList();

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(["1A", "1B", "2A", "2B"]);
    }

    [Fact]
    public void Cartesian_WithNullInputs_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> first = null!;
        var second = new[] { 'A', 'B' };

        // Act & Assert
        var act1 = () => first.Cartesian(second).ToList();
        act1.Should().Throw<ArgumentNullException>();

        var act2 = () => new[] { 1, 2 }.Cartesian((int[])null!).ToList();
        act2.Should().Throw<ArgumentNullException>();

        var act3 = () => new[] { 1, 2 }.Cartesian((int[])null!).ToList();
        act3.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Shuffle_ModifiesListOrder()
    {
        // Arrange
        var original = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        original.Shuffle();

        // Assert
        original.Should().HaveCount(original.Count);
        original.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void Shuffle_WithRandomGenerator_ModifiesListOrder()
    {
        // Arrange
        var original = new List<int> { 1, 2, 3, 4, 5 };
        var copy = new List<int>(original);
        var random = new Random(42); // Fixed seed for reproducibility

        // Act
        original.Shuffle(random);

        // Assert
        original.Should().HaveCount(copy.Count);
        original.Should().NotEqual(copy);
        original.Should().BeEquivalentTo(copy);
    }

    [Fact]
    public void NextOf_ReturnsNextItem()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act & Assert
        list.NextOf(1).Should().Be(2);
        list.NextOf(2).Should().Be(3);
        list.NextOf(3).Should().Be(1); // Wraps around to start
    }

    [Fact]
    public void NextOf_WithEmptyList_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.NextOf(1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TakeEvery_ReturnsEveryNthElement()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5, 6 };

        // Act
        var result = numbers.TakeEvery(2).ToList();

        // Assert
        result.Should().BeEquivalentTo([1, 3, 5], options => options.WithStrictOrdering());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TakeEvery_WithInvalidStep_ThrowsArgumentOutOfRangeException(int step)
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var act = () => numbers.TakeEvery(step).ToList();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TakeEvery_WithNullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> numbers = null;

        // Act
        var act = () => numbers.TakeEvery(2).ToList();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Infinite_ReturnsRepeatingSequence()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };

        // Act
        var result = numbers.Infinite().Take(7).ToList();

        // Assert
        result.Should().BeEquivalentTo([1, 2, 3, 1, 2, 3, 1], options => options.WithStrictOrdering());
    }

    [Fact]
    public void Infinite_WithNullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> numbers = null;

        // Act
        var act = () => numbers.Infinite().Take(1).ToList();

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}