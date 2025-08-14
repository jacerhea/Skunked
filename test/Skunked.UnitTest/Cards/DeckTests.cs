using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Cards;

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
        deck.Should().NotContainInOrder(cardsOriginal);
    }

    [Fact]
    public void SystemTextJson_Is_Able_To_Serialize_And_Deserialize()
    {
        var deckCards = new Deck().ToList();

        var serializedCards = JsonSerializer.Serialize(deckCards);
        var deserializedCards = JsonSerializer.Deserialize<List<Card>>(serializedCards);
        deckCards.Should().ContainInOrder(deserializedCards);
    }
}