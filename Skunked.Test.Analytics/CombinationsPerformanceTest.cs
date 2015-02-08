using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.PlayingCards;

namespace Skunked.Test.Analytics
{
    [TestClass]
    public class CombinationsPerformanceTest
    {
        private List<Card> _hand;

        [TestInitialize]
        public void Setup()
        {
            _hand = new List<Card>
            {
                new Card(Rank.Six, Suit.Clubs),
                new Card(Rank.Eight, Suit.Diamonds),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Eight, Suit.Clubs),
                new Card(Rank.Ace, Suit.Diamonds),
            };
        }

        [TestMethod]
        public void Combinations()
        {
            foreach (var i in Enumerable.Range(0, 1000))
            {
                foreach (int value in Enumerable.Range(1, _hand.Count))
                {
                    var comboGen = new Combinations<Card>(_hand, value);
                    foreach (var set in comboGen)
                    {
                        Assert.IsTrue(set.Count > 0);
                    }
                }
            }
        }
    }
}
