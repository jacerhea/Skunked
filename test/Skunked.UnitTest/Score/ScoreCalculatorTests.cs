using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.Cards;
using Skunked.Score;
using Xunit;

namespace Skunked.UnitTest.Score
{
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
            resultSets.Count.Should().Be(5);

            resultSets[1].Count.Should().Be(5);
            resultSets[2].Count.Should().Be(10);
            resultSets[3].Count.Should().Be(10);
            resultSets[4].Count.Should().Be(5);
            resultSets[5].Count.Should().Be(1);
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
            result.Combinations.Fifteens.Count.Should().Be(2);
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

            var combosMakeRuns = _scoreCalculator.CountRuns(combos);
            combosMakeRuns.Count.Should().Be(2);
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

            var combosMakeRuns = _scoreCalculator.CountRuns(combos);
            combosMakeRuns.Count.Should().Be(2);
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

            var pairsCombinations = _scoreCalculator.CountPairs(combos);
            pairsCombinations.Count.Should().Be(2);
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

            var areSameKind = _scoreCalculator.AreSameKind(hand);

            areSameKind.Should().BeTrue();
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

            var areNotSameKind = _scoreCalculator.AreSameKind(hand);

            areNotSameKind.Should().BeFalse();
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

            var pairsCombinations = _scoreCalculator.CountPairs(combos);
            pairsCombinations.Count.Should().Be(3);
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

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Clubs));
            pairsCombinations.Count.Should().Be(4);
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

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Spades));
            pairsCombinations.Count.Should().Be(5);
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

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Spades));
            pairsCombinations.Count.Should().Be(0);
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
            score.Points.Score.Should().Be(29);
            score.Points.PairScore.Should().Be(12);
            score.Points.NobScore.Should().Be(1);
            score.Points.FifteenScore.Should().Be(16);
            score.Points.FlushScore.Should().Be(0);
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
            scoreResult.Points.Score.Should().Be(24);
            scoreResult.Points.PairScore.Should().Be(4);
            scoreResult.Points.NobScore.Should().Be(0);
            scoreResult.Points.FifteenScore.Should().Be(8);
            scoreResult.Points.FlushScore.Should().Be(0);
            scoreResult.Points.RunScore.Should().Be(12);
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
            totalScore.Should().Be(14);
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
            isFifteen.Should().BeFalse();
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
            isFifteen.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NobsTestData))]
        public void ScoreCalculatorDoesNotHaveNobsTest(IEnumerable<Card> play, Card starter, int expectation)
        {
            var hasNobs = _scoreCalculator.Nobs(play, starter);
            hasNobs.Count.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(ContinousTestData))]
        public void ScoreCalculatorAreContinuous(List<int> play, bool expectation)
        {
            var areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().Be(expectation);
        }

        [Theory]
        [MemberData(nameof(RunTestData))]
        public void FiveSixSeven_Is_A_Run(List<Card> hand, bool expectation)
        {
            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().Be(expectation);
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
                }, new Card(Rank.Three, Suit.Diamonds) ,0 },
                new object[] { new List<Card>
                {
                    new(Rank.Six, Suit.Spades),
                    new(Rank.Seven, Suit.Spades),
                    new(Rank.Queen, Suit.Hearts),
                    new(Rank.Jack, Suit.Clubs)
                }, new Card(Rank.Three, Suit.Clubs), 1 },
            };

        public static IEnumerable<object[]> ContinousTestData =>
            new List<object[]>
            {
                new object[] { new List<int> { 109 } ,true },
                new object[] { new List<int> { 109, 110 }, true },
                new object[] { new List<int> { 110, 111, 109 }, true },
                new object[] { new List<int> { 0, -1 }, true },
                new object[] { new List<int> { 108, 110 }, false },
                new object[] { new List<int> { 110, 107, 108 }, false },
                new object[] { new List<int> { 0, -2 }, false },
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
}

