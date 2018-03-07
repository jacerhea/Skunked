using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Skunked.AI.Play;
using Skunked.PlayingCards;
using Skunked.Rules;
using Xunit;

namespace Skunked.Standard.UnitTest.AI.Play
{
    public class MaxDifferencePlayStrategyTests
    {
        private readonly MaxDifferencePlayStrategy _maxDifferencePlayStrategy = new MaxDifferencePlayStrategy();
        private readonly AceLowFaceTenCardValueStrategy _valueStrategy = new AceLowFaceTenCardValueStrategy();


        [Fact]
        public void Test_LowerCardIsPlayed()
        {
            var gameRules = new GameRules();
            var pile = new List<Card> { new Card(Rank.Eight, Suit.Clubs) };
            var handLeft = new List<Card>
            {
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Six, Suit.Diamonds),
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Five, Suit.Spades),
            };

            var thrown = _maxDifferencePlayStrategy.GetRemainingPossibilities(new MaxDifferencePlayStrategy.PlayState
            {
                Count = _valueStrategy.ValueOf(pile[0]),
                HandLeft = handLeft,
                MyTurn = true,
                Pile = pile
            }).ToList();

               thrown.Should().OnlyHaveUniqueItems();

            foreach (var set in thrown)
            {
                var notSets = thrown.Where(cardSet => cardSet != set).ToList();
                foreach (var notSet in notSets)
                {
                    set.Should().NotEqual(notSet);
                }
                set.Should().OnlyHaveUniqueItems()
                    .And.NotContainNulls();
            }

            var single = thrown.Single(set => AreEqual(set, new List<Card>
            {
                new Card(Rank.Eight, Suit.Clubs),
                new Card(Rank.Queen, Suit.Clubs),
                new Card(Rank.King, Suit.Diamonds),
                new Card(Rank.Three, Suit.Diamonds),
            }));

            //thrown.Contains(handLeft[0]);
        }

        private bool AreEqual<T>(List<T> source1, List<T> source2)
        {
            if (source1.Count != source2.Count)
            {
                return false;
            }

            foreach (var item1 in source1.Select((item, index) => new { item, index }))
            {
                if (!item1.item.Equals(source2[item1.index]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
