using System.Text.Encodings.Web;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Skunked.UnitTest.Cards;

public class CardTests
{
    [Fact]
    public void Card_Constructor_With_Rank_And_Suit_Will_Set_Rank_And_Suit_Correctly()
    {
        var card = new Card(Rank.King, Suit.Hearts);
        card.Rank.Should().Be(Rank.King);
        card.Suit.Should().Be(Suit.Hearts);
    }

    [Fact]
    public void Card_Constructor_With_Card_Parameter_Will_Set_Rank_And_Suit_Correctly()
    {
        var original = new Card(Rank.Four, Suit.Spades);
        var testCard = new Card(original);
        original.Rank.Should().Be(Rank.Four);
        original.Suit.Should().Be(Suit.Spades);
        original.Should().Be(testCard);
    }

    [Fact]
    public void SystemTextJson_Is_Able_To_Serialize_And_Deserialize()
    {
        var original = new Card(Rank.Four, Suit.Spades);

        var serializedCard = JsonSerializer.Serialize(original);
        var deserializedCard = JsonSerializer.Deserialize<Card>(serializedCard);
        original.Rank.Should().Be(Rank.Four);
        original.Suit.Should().Be(Suit.Spades);
        deserializedCard.Should().Be(original);
    }
}