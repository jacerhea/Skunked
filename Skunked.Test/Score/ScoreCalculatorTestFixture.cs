using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.PlayingCards;
using Skunked.Score;

namespace Skunked.Test.Score
{
    [TestClass]
    public class ScoreCalculatorTestFixture
    {
        private ScoreCalculator _scoreCalculator;

        [TestInitialize]
        public void Setup()
        {
            _scoreCalculator = new ScoreCalculator();
        }

        [TestMethod]
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
            Assert.AreEqual(5, resultSets.Count);

            Assert.AreEqual(5, resultSets[1].Count);
            Assert.AreEqual(10, resultSets[2].Count);
            Assert.AreEqual(10, resultSets[3].Count);
            Assert.AreEqual(5, resultSets[4].Count);
            Assert.AreEqual(1, resultSets[5].Count);
        }

        [TestMethod]
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
            Assert.AreEqual(2, result.Fifteens.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(2, combosMakeRuns.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(2, combosMakeRuns.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(2, pairsCombinations.Count);
        }

        [TestMethod]
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

            Assert.IsTrue(areSameKind);
        }

        [TestMethod]
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

            Assert.IsFalse(areNotSameKind);
        }

        [TestMethod]
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
            Assert.AreEqual(3, pairsCombinations.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(4, pairsCombinations.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(5, pairsCombinations.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(0, pairsCombinations.Count);
        }

        [TestMethod]
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
            Assert.AreEqual(29, score.Score);
            Assert.AreEqual(12, score.PairScore);
            Assert.AreEqual(1, score.NobScore);
            Assert.AreEqual(16, score.FifteenScore);
            Assert.AreEqual(0, score.FlushScore);
            Assert.AreEqual(0, score.RunScore);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptionsForRunTest()
        {
            _scoreCalculator.CountRuns(null);
        }

        [TestMethod]
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
            Assert.AreEqual(24, scoreResult.Score);
            Assert.AreEqual(4, scoreResult.PairScore);
            Assert.AreEqual(0, scoreResult.NobScore);
            Assert.AreEqual(8, scoreResult.FifteenScore);
            Assert.AreEqual(0, scoreResult.FlushScore);
            Assert.AreEqual(12, scoreResult.RunScore);
        }

        [TestMethod]
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
            Assert.AreEqual(14, totalScore);
        }

        [TestMethod]
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
            Assert.IsFalse(isFifteen);
        }

        [TestMethod]
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
            Assert.IsTrue(isFifteen);
        }

        [TestMethod]
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
            Assert.IsTrue(nobs.Count == 1);
        }

        [TestMethod]
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
            Assert.IsTrue(hasNobs.Count == 0);
        }

        [TestMethod]
        public void ScoreCalculatoAreContinousTestOne()
        {
            var play = new List<int> { 109 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsTrue(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatorAreContinousTestTwo()
        {
            var play = new List<int> { 109, 110 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsTrue(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatoAreContinousTestThree()
        {
            var play = new List<int> { 110, 111, 109 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsTrue(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatoAreContinousTest()
        {
            var play = new List<int> { 0, -1 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsTrue(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatorAreNotContinousTestTwo()
        {
            var play = new List<int> { 108, 110 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsFalse(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatoAreNotContinousTestThree()
        {
            var play = new List<int> { 110, 107, 108 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsFalse(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatoAreNotContinousTest()
        {
            var play = new List<int> { 0, -2 };

            bool areContinuous = _scoreCalculator.AreContinuous(play);
            Assert.IsFalse(areContinuous);
        }

        [TestMethod]
        public void ScoreCalculatorIsRun()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Clubs),
                               new Card(Rank.Five, Suit.Hearts),
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            Assert.IsTrue(isRun);
        }

        [TestMethod]
        public void ScoreCalculatorIsRunAll()
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
            Assert.IsTrue(isRun);
        }

        [TestMethod]
        public void ScoreCalculatorIsNotRunMissingSix()
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
            Assert.IsFalse(isRun);
        }

        [TestMethod]
        public void ScoreCalculatorIsRunFaceCards()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Jack, Suit.Spades),
                               new Card(Rank.King, Suit.Clubs),
                               new Card(Rank.Queen, Suit.Hearts),
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            Assert.IsTrue(isRun);
        }

        [TestMethod]
        public void ScoreCalculatorIsNotRun()
        {
            var hand = new List<Card>
                           {
                               new Card(Rank.Six, Suit.Spades),
                               new Card(Rank.Seven, Suit.Clubs),
                               new Card(Rank.Five, Suit.Hearts),
                               new Card(Rank.Three, Suit.Diamonds)
                           };

            var isRun = _scoreCalculator.IsRun(hand);
            Assert.IsFalse(isRun);
        }
    }
}