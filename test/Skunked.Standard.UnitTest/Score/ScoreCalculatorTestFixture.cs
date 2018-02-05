using System;
using System.Collections.Generic;
using FluentAssertions;
using Skunked.PlayingCards;
using Skunked.Score;
using Xunit;

namespace Skunked.Test.Score
{
    public class ScoreCalculatorTestFixture
    {
        private ScoreCalculator _scoreCalculator;

        public ScoreCalculatorTestFixture()
        {
            _scoreCalculator = new ScoreCalculator();
        }

        [Fact]
        public void ScoreCalculatorTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Two, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Seven, Suit.Hearts),
                               new Card(Rank.Six, Suit.Diamonds)
                           };

            var resultSets = _scoreCalculator.GetCombinations(hand);
            Assert.Equal(5, resultSets.Count);

            Assert.Equal(5, resultSets[1].Count);
            Assert.Equal(10, resultSets[2].Count);
            Assert.Equal(10, resultSets[3].Count);
            Assert.Equal(5, resultSets[4].Count);
            Assert.Equal(1, resultSets[5].Count);
        }

        [Fact]
        public void CountFifteenTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Seven, Suit.Hearts),
                               new Card(Rank.Six, Suit.Diamonds)
                           };

            var result = _scoreCalculator.CountShowScore(new Card(Rank.Two, Suit.Spades), hand);
            Assert.Equal(2, result.Fifteens.Count);
        }

        [Fact]
        public void CountThreeRunsTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Two, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Six, Suit.Diamonds)
                           };

            var combos = _scoreCalculator.GetCombinations(hand);

            var combosMakeRuns = _scoreCalculator.CountRuns(combos);
            Assert.Equal(2, combosMakeRuns.Count);
        }

        [Fact]
        public void CountFourRunsTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Ace, Suit.Diamonds)
                           };

            var combos = _scoreCalculator.GetCombinations(hand);

            var combosMakeRuns = _scoreCalculator.CountRuns(combos);
            Assert.Equal(2, combosMakeRuns.Count);
        }

        [Fact]
        public void CountTwoPairsTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Four, Suit.Clubs),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Ace, Suit.Diamonds)
                           };

            var combos = _scoreCalculator.GetCombinations(hand);

            var pairsCombinations = _scoreCalculator.CountPairs(combos);
            Assert.Equal(2, pairsCombinations.Count);
        }

        [Fact]
        public void AreSameKindTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Nine, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Diamonds),
                               new Card(Rank.Nine, Suit.Hearts),
                               new Card(Rank.Nine, Suit.Spades)
                           };

            var areSameKind = _scoreCalculator.AreSameKind(hand);

            Assert.True(areSameKind);
        }

        [Fact]
        public void AreNotSameKindTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Nine, Suit.Clubs),
                               new Card(Rank.Nine, Suit.Diamonds),
                               new Card(Rank.Nine, Suit.Hearts),
                               new Card(Rank.Ten, Suit.Spades)
                           };

            var areNotSameKind = _scoreCalculator.AreSameKind(hand);

            Assert.False(areNotSameKind);
        }

        [Fact]
        public void CountThreePairsTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Ace, Suit.Clubs),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Ace, Suit.Diamonds)
                           };

            var combos = _scoreCalculator.GetCombinations(hand);

            var pairsCombinations = _scoreCalculator.CountPairs(combos);
            Assert.Equal(3, pairsCombinations.Count);
        }

        [Fact]
        public void Count4FlushTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Eight, Suit.Spades),
                               new Card(Rank.Four, Suit.Spades)
                           };

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Clubs));
            Assert.Equal(4, pairsCombinations.Count);
        }

        [Fact]
        public void Count5FlushTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Eight, Suit.Spades),
                               new Card(Rank.Four, Suit.Spades)
                           };

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Spades));
            Assert.Equal(5, pairsCombinations.Count);
        }

        [Fact]
        public void NotFlushTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Three, Suit.Spades),
                               new Card(Rank.Eight, Suit.Spades),
                               new Card(Rank.Four, Suit.Diamonds)
                           };

            var pairsCombinations = _scoreCalculator.CountFlush(hand, new Card(Rank.Queen, Suit.Spades));
            Assert.Equal(0, pairsCombinations.Count);
        }

        [Fact]
        public void Count29Test()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Five, Suit.Clubs),
                               new Card(Rank.Five, Suit.Diamonds),
                               new Card(Rank.Five, Suit.Hearts),
                               new Card(Rank.Jack, Suit.Spades)
                           };

            var score = _scoreCalculator.CountShowScore(new Card(Rank.Five, Suit.Spades), hand);
            score.Score.Should().Be(29);
            score.PairScore.Should().Be(12);
            score.NobScore.Should().Be(1);
            score.FifteenScore.Should().Be(16);
            score.FlushScore.Should().Be(0);
        }


        [Fact]
        public void ExceptionsForRunTest()
        {
            Action countRuns = () => _scoreCalculator.CountRuns(null);
            countRuns.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ScoreCalculatorTotalTest()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Spades),
                               new Card(Rank.Eight, Suit.Clubs),
                               new Card(Rank.Eight, Suit.Hearts)
                           };

            var scoreResult = _scoreCalculator.CountShowScore(new Card(Rank.Seven, Suit.Diamonds), hand);
            scoreResult.Score.Should().Be(24);
            scoreResult.PairScore.Should().Be(4);
            scoreResult.NobScore.Should().Be(0);
            scoreResult.FifteenScore.Should().Be(8);
            scoreResult.FlushScore.Should().Be(0);
            scoreResult.RunScore.Should().Be(12);
        }

        [Fact]
        public void ScoreCalculatorCountThePlayTest()
        {
            var play = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Spades),
                               new Card(Rank.Queen, Suit.Hearts),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Two, Suit.Diamonds),
                               new Card(Rank.Two, Suit.Hearts),
                               new Card(Rank.Two, Suit.Spades)
                           };

            var totalScore = _scoreCalculator.CountThePlay(play);
            totalScore.Should().Be(14);
        }

        [Fact]
        public void IsNotFifteenTest()
        {
            var play = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Two, Suit.Spades),
                               new Card(Rank.Three, Suit.Hearts),
                               new Card(Rank.Four, Suit.Clubs),
                           };

            var isFifteen = _scoreCalculator.IsFifteen(play);
            isFifteen.Should().BeFalse();
        }

        [Fact]
        public void IsFifteenTest()
        {
            var play = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Two, Suit.Spades),
                               new Card(Rank.Three, Suit.Hearts),
                               new Card(Rank.Four, Suit.Clubs),
                               new Card(Rank.Five, Suit.Diamonds),
                           };

            var isFifteen = _scoreCalculator.IsFifteen(play);
            isFifteen.Should().BeTrue();
        }

        [Fact]
        public void ScoreCalculatorHasNobsTest()
        {
            var play = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Spades),
                               new Card(Rank.Queen, Suit.Hearts),
                               new Card(Rank.Jack, Suit.Clubs),
                           };

            var nobs = _scoreCalculator.Nobs(play, new Card(Rank.Three, Suit.Clubs));
            nobs.Count.Should().Be(1);
        }

        [Fact]
        public void ScoreCalculatorDoesNotHaveNobsTest()
        {
            var play = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Spades),
                               new Card(Rank.Queen, Suit.Hearts),
                               new Card(Rank.Jack, Suit.Clubs),
                           };

            var hasNobs = _scoreCalculator.Nobs(play, new Card(Rank.Three, Suit.Diamonds));
            hasNobs.Count.Should().Be(0);
        }

        [Fact]
        public void ScoreCalculatoAreContinousTestOne()
        {
            var play = new List<int> { 109 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeTrue();
        }

        [Fact]
        public void ScoreCalculatorAreContinousTestTwo()
        {
            var play = new List<int> { 109, 110 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeTrue();
        }

        [Fact]
        public void ScoreCalculatoAreContinousTestThree()
        {
            var play = new List<int> { 110, 111, 109 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeTrue();
        }

        [Fact]
        public void ScoreCalculatoAreContinousTest()
        {
            var play = new List<int> { 0, -1 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeTrue();
        }

        [Fact]
        public void ScoreCalculatorAreNotContinousTestTwo()
        {
            var play = new List<int> { 108, 110 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeFalse();
        }

        [Fact]
        public void ScoreCalculatoAreNotContinousTestThree()
        {
            var play = new List<int> { 110, 107, 108 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeFalse();
        }

        [Fact]
        public void Zero_NegativeTwo_Are_Not_Continous()
        {
            var play = new List<int> { 0, -2 };
            var areContinuous = _scoreCalculator.AreContinuous(play);
            areContinuous.Should().BeFalse();
        }

        [Fact]
        public void FiveSixSeven_Is_A_Run()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Clubs),
                               new Card(Rank.Five, Suit.Hearts),
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().BeTrue();
        }

        [Fact]
        public void AceThroughKing_Is_A_Run()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Three, Suit.Hearts),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Five, Suit.Hearts),
                               new Card(Rank.Six, Suit.Hearts),
                               new Card(Rank.Seven, Suit.Hearts),
                               new Card(Rank.Eight, Suit.Hearts),
                               new Card(Rank.Nine, Suit.Hearts),
                               new Card(Rank.Ten, Suit.Hearts),
                               new Card(Rank.Jack, Suit.Hearts),
                               new Card(Rank.Queen, Suit.Hearts),
                               new Card(Rank.King, Suit.Hearts)
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().BeTrue();
        }

        [Fact]
        public void AceThroughKing_Except_Six_Is_Not_A_Run()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Ace, Suit.Spades),
                               new Card(Rank.Two, Suit.Clubs),
                               new Card(Rank.Three, Suit.Hearts),
                               new Card(Rank.Four, Suit.Hearts),
                               new Card(Rank.Five, Suit.Hearts),
                               new Card(Rank.Seven, Suit.Hearts),
                               new Card(Rank.Eight, Suit.Hearts),
                               new Card(Rank.Nine, Suit.Hearts),
                               new Card(Rank.Ten, Suit.Hearts),
                               new Card(Rank.Jack, Suit.Hearts),
                               new Card(Rank.Queen, Suit.Hearts),
                               new Card(Rank.King, Suit.Hearts)
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().BeFalse();
        }

        [Fact]
        public void JackQueenKing_Is_A_Run()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Jack, Suit.Spades),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Hearts),
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().BeTrue();
        }

        [Fact]
        public void ThreeFiveSixSeven_Is_Not_A_Run()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Clubs),
                               new Card(Rank.Five, Suit.Hearts),
                               new Card(Rank.Three, Suit.Diamonds)
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            isRun.Should().BeFalse();
        }
    }
}