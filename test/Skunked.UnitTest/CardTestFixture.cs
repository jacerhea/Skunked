﻿using System;
using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Utility;
using Xunit;

namespace Skunked.UnitTest;

public class CardTestFixture
{
    [Fact]
    public void TestCardProperties()
    {
        IEnumerable<Rank> allRanks = Enum.GetValues<Rank>();
        IEnumerable<Suit> allSuits = Enum.GetValues<Suit>();

        foreach (var rank in allRanks)
        {
            foreach (var suit in allSuits)
            {
                var card = new Card(rank, suit);
                Assert.Equal(rank, card.Rank);
                Assert.Equal(suit, card.Suit);
            }
        }
    }

    [Fact]
    public void TestCardEqualsTyped()
    {
        IEnumerable<Rank> allRanks = Enum.GetValues<Rank>();
        IEnumerable<Suit> allSuits = Enum.GetValues<Suit>();

        foreach (var rank in allRanks)
        {
            foreach (var suit in allSuits)
            {
                var card = new Card(rank, suit);
                var clonedCard = new Card(rank, suit);

                Assert.True(card.Equals(clonedCard));
            }
        }
    }
}