using System.Text.Json;
using Xunit;

namespace Skunked.UnitTest.Cards;

public class DeckTests
{
    [Fact]
    public void Deck_Has_52_Cards()
    {
        var deck = new Deck();
        Assert.Equal(52, deck.Count());
    }

    [Fact]
    public void Deck_Cards_Are_All_Unique()
    {
        var deck = new Deck();
        Assert.Equal(deck.Count(), deck.Distinct().Count());
    }

    [Fact]
    public void Deck_Shuffle_Will_Rearrange_Cards()
    {
        var deck = new Deck();
        var cardsOriginal = deck.ToList();
        deck.Shuffle();
        Assert.NotEqual(cardsOriginal, deck.ToList());
    }

    [Fact]
    public void SystemTextJson_Is_Able_To_Serialize_And_Deserialize()
    {
        var deckCards = new Deck().ToList();

        var serializedCards = JsonSerializer.Serialize(deckCards);
        var deserializedCards = JsonSerializer.Deserialize<List<Card>>(serializedCards);
        Assert.Equal(deckCards, deserializedCards);
    }
}