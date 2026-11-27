using System;
using System.Collections.Generic;
using Xunit;

namespace Skunked.UnitTest.Players
{
    public class PlayerHandTests
    {
        [Fact]
        public void Constructor_Sets_PlayerId_And_Hand_Instance()
        {
            // Arrange
            var handList = new List<Card>
            {
                new(Rank.Five, Suit.Clubs),
                new(Rank.King, Suit.Hearts)
            };

            // Act
            var sut = new PlayerHand(42, handList);

            // Assert
            Assert.Equal(42, sut.PlayerId);
            Assert.Same(handList, sut.Hand);
            Assert.Equal(handList, sut.Hand);
        }

        [Fact]
        public void Constructor_With_Null_Hand_Throws_ArgumentNullException()
        {
            // Arrange
            List<Card> hand = null!;

            // Act
            Action act = () => new PlayerHand(1, hand);

            // Assert
            Assert.Equal("hand", Assert.ThrowsAny<ArgumentNullException>(act).ParamName);
        }

        [Fact]
        public void Constructor_Allows_Empty_Hand()
        {
            // Act
            var sut = new PlayerHand(1, new List<Card>());

            // Assert
            Assert.NotNull(sut.Hand);
            Assert.Empty(sut.Hand);
        }

        [Fact]
        public void Hand_List_Is_Not_Copied_And_Remains_Mutable()
        {
            // Arrange
            var original = new List<Card> { new(Rank.Ace, Suit.Spades) };

            // Act
            var sut = new PlayerHand(7, original);

            // Assert - modifying original reflects in PlayerHand.Hand
            original.Add(new Card(Rank.Two, Suit.Spades));
            Assert.Equal(2, sut.Hand.Count);
            Assert.Contains(new Card(Rank.Two, Suit.Spades), sut.Hand);

            // Assert - modifying PlayerHand.Hand reflects in original
            sut.Hand.Add(new Card(Rank.Three, Suit.Spades));
            Assert.Equal(3, original.Count);
            Assert.Contains(new Card(Rank.Three, Suit.Spades), original);
        }
    }
}
