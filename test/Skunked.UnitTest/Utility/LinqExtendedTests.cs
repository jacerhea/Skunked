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
        Assert.Equal(4, result.Count);
        Assert.All(new[] { "1A", "1B", "2A", "2B" }, item => Assert.Contains(item, result));
    }

    [Fact]
    public void Cartesian_WithNullInputs_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> first = null!;
        var second = new[] { 'A', 'B' };

        // Act & Assert
        var act1 = () => first.Cartesian(second).ToList();
        Assert.ThrowsAny<ArgumentNullException>(act1);

        var act2 = () => new[] { 1, 2 }.Cartesian((int[])null!).ToList();
        Assert.ThrowsAny<ArgumentNullException>(act2);

        var act3 = () => new[] { 1, 2 }.Cartesian((int[])null!).ToList();
        Assert.ThrowsAny<ArgumentNullException>(act3);
    }

    [Fact]
    public void Shuffle_ModifiesListOrder()
    {
        // Arrange
        var original = new List<int> { 1, 2, 3, 4, 5 };
        var copy = original.ToList();

        // Act
        original.Shuffle();

        // Assert
        Assert.Equal(copy.Count, original.Count);
        Assert.Equal(copy.OrderBy(x => x), original.OrderBy(x => x));
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
        Assert.Equal(copy.Count, original.Count);
        Assert.NotEqual(copy, original);
        Assert.Equal(copy.OrderBy(x => x), original.OrderBy(x => x));
    }

    [Fact]
    public void NextOf_ReturnsNextItem()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act & Assert
        Assert.Equal(2, list.NextOf(1));
        Assert.Equal(3, list.NextOf(2));
        Assert.Equal(1, list.NextOf(3)); // Wraps around to start
    }

    [Fact]
    public void NextOf_WithEmptyList_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var act = () => list.NextOf(1);

        // Assert
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => { _ = act(); });
    }

    [Fact]
    public void TakeEvery_ReturnsEveryNthElement()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5, 6 };

        // Act
        var result = numbers.TakeEvery(2).ToList();

        // Assert
        Assert.Equal(new[] {1, 3, 5}, result);
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
        Assert.ThrowsAny<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void TakeEvery_WithNullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> numbers = null!;

        // Act
        var act = () => numbers.TakeEvery(2).ToList();

        // Assert
        Assert.ThrowsAny<ArgumentNullException>(act);
    }

    [Fact]
    public void Infinite_ReturnsRepeatingSequence()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };

        // Act
        var result = numbers.Infinite().Take(7).ToList();

        // Assert
        Assert.Equal(new[] {1, 2, 3, 1, 2, 3, 1}, result);
    }

    [Fact]
    public void Infinite_WithNullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> numbers = null!;

        // Act
        var act = () => numbers.Infinite().Take(1).ToList();

        // Assert
        Assert.ThrowsAny<ArgumentNullException>(act);
    }
}
