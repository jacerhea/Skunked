using System.Linq;
using FluentAssertions;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Standard.UnitTest.PlayingCards
{
    public class DeckTests
    {
        [Fact]
        public void Deck_Has_52_Cards()
        {
            var deck = new Deck();
            deck.Count().Should().Be(52);
        }

        [Fact]
        public void Deck_Cards_Are_All_Unique()
        {
            var deck = new Deck();
            deck.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Deck_Shuffle_Will_Rearrange_Cards()
        {
            var deck = new Deck();
            var cardsOriginal = deck.ToList();
            deck.Shuffle();
        }
    }
}
