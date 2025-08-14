using System;
using System.Collections.Generic;
using FluentAssertions;
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
            sut.PlayerId.Should().Be(42);
            sut.Hand.Should().BeSameAs(handList);
            sut.Hand.Should().ContainInOrder(handList);
        }

        [Fact]
        public void Constructor_With_Null_Hand_Throws_ArgumentNullException()
        {
            // Arrange
            List<Card> hand = null!;

            // Act
            Action act = () => new PlayerHand(1, hand);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("hand");
        }

        [Fact]
        public void Constructor_Allows_Empty_Hand()
        {
            // Act
            var sut = new PlayerHand(1, new List<Card>());

            // Assert
            sut.Hand.Should().NotBeNull();
            sut.Hand.Should().BeEmpty();
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
            sut.Hand.Should().HaveCount(2)
               .And.Contain(new Card(Rank.Two, Suit.Spades));

            // Assert - modifying PlayerHand.Hand reflects in original
            sut.Hand.Add(new Card(Rank.Three, Suit.Spades));
            original.Should().HaveCount(3)
                .And.Contain(new Card(Rank.Three, Suit.Spades));
        }
    }
}