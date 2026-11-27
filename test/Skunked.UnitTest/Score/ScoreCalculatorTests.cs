using Xunit;

namespace Skunked.UnitTest.Score;

public class ScoreCalculatorTests
{
    private readonly ScoreCalculator _scoreCalculator;

    public ScoreCalculatorTests()
    {
        _scoreCalculator = new ScoreCalculator();
    }

    [Fact]
    public void ScoreCalculatorTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Two, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Seven, Suit.Hearts),
            new(Rank.Six, Suit.Diamonds)
        };

        var resultSets = _scoreCalculator.GetCombinations(hand);
        Assert.Equal(5, resultSets.Count);

        Assert.Equal(5, resultSets[1].Count);
        Assert.Equal(10, resultSets[2].Count);
        Assert.Equal(10, resultSets[3].Count);
        Assert.Equal(5, resultSets[4].Count);
        Assert.Single(resultSets[5]);
    }

    [Fact]
    public void CountFifteenTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Seven, Suit.Hearts),
            new(Rank.Six, Suit.Diamonds)
        };

        var result = _scoreCalculator.CountShowPoints(new Card(Rank.Two, Suit.Spades), hand);
        Assert.Equal(2, result.Combinations.Fifteens.Count);
    }

    [Fact]
    public void CountThreeRunsTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Two, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Six, Suit.Diamonds)
        };

        var combos = _scoreCalculator.GetCombinations(hand);

        var combosMakeRuns = _scoreCalculator.FindRuns(combos);
        Assert.Equal(2, combosMakeRuns.Count);
    }

    [Fact]
    public void CountFourRunsTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Ace, Suit.Diamonds)
        };

        var combos = _scoreCalculator.GetCombinations(hand);

        var combosMakeRuns = _scoreCalculator.FindRuns(combos);
        Assert.Equal(2, combosMakeRuns.Count);
    }

    [Fact]
    public void CountTwoPairsTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Ace, Suit.Diamonds)
        };

        var combos = _scoreCalculator.GetCombinations(hand);

        var pairsCombinations = _scoreCalculator.FindPairs(combos);
        Assert.NotNull(pairsCombinations);
        Assert.Equal(2, pairsCombinations.Count);
    }

    [Fact]
    public void AreSameKindTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Nine, Suit.Hearts),
            new(Rank.Nine, Suit.Spades)
        };

        var areSameKind = _scoreCalculator.IsSameKind(hand);

        Assert.True(areSameKind);
    }

    [Fact]
    public void AreNotSameKindTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Nine, Suit.Clubs),
            new(Rank.Nine, Suit.Diamonds),
            new(Rank.Nine, Suit.Hearts),
            new(Rank.Ten, Suit.Spades)
        };

        var areNotSameKind = _scoreCalculator.IsSameKind(hand);

        Assert.False(areNotSameKind);
    }

    [Fact]
    public void CountThreePairsTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Ace, Suit.Clubs),
            new(Rank.Four, Suit.Hearts),
            new(Rank.Ace, Suit.Diamonds)
        };

        var combos = _scoreCalculator.GetCombinations(hand);

        var pairsCombinations = _scoreCalculator.FindPairs(combos);
        Assert.NotNull(pairsCombinations);
        Assert.Equal(3, pairsCombinations.Count);
    }

    [Fact]
    public void Count4FlushTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Eight, Suit.Spades),
            new(Rank.Four, Suit.Spades)
        };

        var pairsCombinations = _scoreCalculator.FindFlush(hand, new Card(Rank.Queen, Suit.Clubs));
        Assert.NotNull(pairsCombinations);
        Assert.Equal(4, pairsCombinations.Count);
    }

    [Fact]
    public void Count5FlushTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Eight, Suit.Spades),
            new(Rank.Four, Suit.Spades)
        };

        var pairsCombinations = _scoreCalculator.FindFlush(hand, new Card(Rank.Queen, Suit.Spades));
        Assert.NotNull(pairsCombinations);
        Assert.Equal(5, pairsCombinations.Count);
    }

    [Fact]
    public void Non_Flush_Is_Not_Counted_As_A_Flush()
    {
        var hand = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Three, Suit.Spades),
            new(Rank.Eight, Suit.Spades),
            new(Rank.Four, Suit.Diamonds)
        };

        var pairsCombinations = _scoreCalculator.FindFlush(hand, new Card(Rank.Queen, Suit.Spades));
        Assert.Null(pairsCombinations);
    }

    [Fact]
    public void Elusive_29_Hand_Counts_As_29()
    {
        var hand = new List<Card>
        {
            new(Rank.Five, Suit.Clubs),
            new(Rank.Five, Suit.Diamonds),
            new(Rank.Five, Suit.Hearts),
            new(Rank.Jack, Suit.Spades)
        };

        var score = _scoreCalculator.CountShowPoints(new Card(Rank.Five, Suit.Spades), hand);
        Assert.Equal(29, score.Points.Score);
        Assert.Equal(12, score.Points.PairScore);
        Assert.Equal(1, score.Points.NobScore);
        Assert.Equal(16, score.Points.FifteenScore);
        Assert.Equal(0, score.Points.FlushScore);
    }


    [Fact]
    public void ScoreCalculatorTotalTest()
    {
        var hand = new List<Card>
        {
            new(Rank.Six, Suit.Spades),
            new(Rank.Seven, Suit.Spades),
            new(Rank.Eight, Suit.Clubs),
            new(Rank.Eight, Suit.Hearts)
        };

        var scoreResult = _scoreCalculator.CountShowPoints(new Card(Rank.Seven, Suit.Diamonds), hand);
        Assert.Equal(24, scoreResult.Points.Score);
        Assert.Equal(4, scoreResult.Points.PairScore);
        Assert.Equal(0, scoreResult.Points.NobScore);
        Assert.Equal(8, scoreResult.Points.FifteenScore);
        Assert.Equal(0, scoreResult.Points.FlushScore);
        Assert.Equal(12, scoreResult.Points.RunScore);
    }

    [Fact]
    public void ScoreCalculatorCountThePlayTest()
    {
        var play = new List<Card>
        {
            new(Rank.Six, Suit.Spades),
            new(Rank.Seven, Suit.Spades),
            new(Rank.Queen, Suit.Hearts),
            new(Rank.Two, Suit.Clubs),
            new(Rank.Two, Suit.Diamonds),
            new(Rank.Two, Suit.Hearts),
            new(Rank.Two, Suit.Spades)
        };

        var totalScore = _scoreCalculator.CountPlayPoints(play);
        Assert.Equal(14, totalScore);
    }

    [Fact]
    public void IsNotFifteenTest()
    {
        var play = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Four, Suit.Clubs)
        };

        var isFifteen = _scoreCalculator.IsFifteen(play);
        Assert.False(isFifteen);
    }

    [Fact]
    public void IsFifteenTest()
    {
        var play = new List<Card>
        {
            new(Rank.Ace, Suit.Spades),
            new(Rank.Two, Suit.Spades),
            new(Rank.Three, Suit.Hearts),
            new(Rank.Four, Suit.Clubs),
            new(Rank.Five, Suit.Diamonds)
        };

        var isFifteen = _scoreCalculator.IsFifteen(play);
        Assert.True(isFifteen);
    }

    [Theory]
    [MemberData(nameof(NobsTestData))]
    public void ScoreCalculatorDoesNotHaveNobsTest(IEnumerable<Card> play, Card starter, Card expectation)
    {
        var hasNobs = _scoreCalculator.FindNobs(play, starter);
        Assert.Equal(expectation, hasNobs);
    }

    [Theory]
    [MemberData(nameof(ContinuousTestData))]
    public void ScoreCalculatorAreContinuous(List<int> play, bool expectation)
    {
        var areContinuous = _scoreCalculator.IsContinuous(play);
        Assert.Equal(expectation, areContinuous);
    }

    [Theory]
    [MemberData(nameof(RunTestData))]
    public void FiveSixSeven_Is_A_Run(List<Card> hand, bool expectation)
    {
        var isRun = _scoreCalculator.IsRun(hand);
        Assert.Equal(expectation, isRun);
    }

    public static IEnumerable<object[]> NobsTestData =>
        new List<object[]>
        {
            new object[] { new List<Card>
            {
                new(Rank.Six, Suit.Spades),
                new(Rank.Seven, Suit.Spades),
                new(Rank.Queen, Suit.Hearts),
                new(Rank.Jack, Suit.Clubs)
            }, new Card(Rank.Three, Suit.Diamonds) , null! },
            new object[] { new List<Card>
            {
                new(Rank.Six, Suit.Spades),
                new(Rank.Seven, Suit.Spades),
                new(Rank.Queen, Suit.Hearts),
                new(Rank.Jack, Suit.Clubs)
            }, new Card(Rank.Three, Suit.Clubs), new Card(Rank.Jack, Suit.Clubs)  },
        };

    public static IEnumerable<object[]> ContinuousTestData =>
        new List<object[]>
        {
            new object[] { new List<int> { 109 } ,true },
            new object[] { new List<int> { 109, 110 }, true },
            new object[] { new List<int> { 110, 111, 109 }, true },
            new object[] { new List<int> { 0, -1 }, true },
            new object[] { new List<int> { 108, 110 }, false },
            new object[] { new List<int> { 110, 107, 108 }, false },
            new object[] { new List<int> { 0, -2 }, false },
            new object[] { new List<int> { 0, 0 }, false },
        };


    public static IEnumerable<object[]> RunTestData =>
        new List<object[]>
        {
            new object[] { new List<Card>
            {
                new(Rank.Six, Suit.Spades),
                new(Rank.Seven, Suit.Clubs),
                new(Rank.Five, Suit.Hearts)
            } ,true },
            new object[] { new List<Card>
            {
                new(Rank.Ace, Suit.Spades),
                new(Rank.Two, Suit.Clubs),
                new(Rank.Three, Suit.Hearts),
                new(Rank.Four, Suit.Hearts),
                new(Rank.Five, Suit.Hearts),
                new(Rank.Six, Suit.Hearts),
                new(Rank.Seven, Suit.Hearts),
                new(Rank.Eight, Suit.Hearts),
                new(Rank.Nine, Suit.Hearts),
                new(Rank.Ten, Suit.Hearts),
                new(Rank.Jack, Suit.Hearts),
                new(Rank.Queen, Suit.Hearts),
                new(Rank.King, Suit.Hearts)
            } ,true },
            new object[] { new List<Card>
            {
                new(Rank.Ace, Suit.Spades),
                new(Rank.Two, Suit.Clubs),
                new(Rank.Three, Suit.Hearts),
                new(Rank.Four, Suit.Hearts),
                new(Rank.Five, Suit.Hearts),
                new(Rank.Seven, Suit.Hearts),
                new(Rank.Eight, Suit.Hearts),
                new(Rank.Nine, Suit.Hearts),
                new(Rank.Ten, Suit.Hearts),
                new(Rank.Jack, Suit.Hearts),
                new(Rank.Queen, Suit.Hearts),
                new(Rank.King, Suit.Hearts)
            } ,false },
            new object[] { new List<Card>
            {
                new(Rank.Jack, Suit.Spades),
                new(Rank.King, Suit.Clubs),
                new(Rank.Queen, Suit.Hearts)
            },true },
            new object[] { new List<Card>
            {
                new(Rank.Six, Suit.Spades),
                new(Rank.Seven, Suit.Clubs),
                new(Rank.Five, Suit.Hearts),
                new(Rank.Three, Suit.Diamonds)
            } ,false }
        };
}
