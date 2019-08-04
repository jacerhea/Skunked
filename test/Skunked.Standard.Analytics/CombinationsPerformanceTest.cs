using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Standard.Analytics
{
    public class CombinationsPerformanceTest
    {
        private readonly List<Card> _hand;

        public CombinationsPerformanceTest()
        {
            _hand = new List<Card>
            {
                new Card(Rank.Six, Suit.Clubs),
                new Card(Rank.Eight, Suit.Diamonds),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Eight, Suit.Clubs),
                new Card(Rank.Ace, Suit.Diamonds)
            };            
        }

        [Fact]
        public void Combinations()
        {
            foreach (var i in Enumerable.Range(0, 1000))
            {
                foreach (int value in Enumerable.Range(1, _hand.Count))
                {
                    var comboGen = new Combinations<Card>(_hand, value);
                    foreach (var set in comboGen)
                    {
                        Assert.True(set.Count > 0);
                    }
                }
            }
        }
    }
}
