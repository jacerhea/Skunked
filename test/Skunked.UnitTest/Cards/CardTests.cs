using System.Text.Encodings.Web;
using System.Text.Json;
using Xunit;

namespace Skunked.UnitTest.Cards;

public class CardTests
{
    [Fact]
    public void Card_Constructor_With_Rank_And_Suit_Will_Set_Rank_And_Suit_Correctly()
    {
        var card = new Card(Rank.King, Suit.Hearts);
        Assert.Equal(Rank.King, card.Rank);
        Assert.Equal(Suit.Hearts, card.Suit);
    }

    [Fact]
    public void Card_Constructor_With_Card_Parameter_Will_Set_Rank_And_Suit_Correctly()
    {
        var original = new Card(Rank.Four, Suit.Spades);
        var testCard = new Card(original);
        Assert.Equal(Rank.Four, original.Rank);
        Assert.Equal(Suit.Spades, original.Suit);
        Assert.Equal(testCard, original);
    }

    [Fact]
    public void SystemTextJson_Is_Able_To_Serialize_And_Deserialize()
    {
        var original = new Card(Rank.Four, Suit.Spades);

        var serializedCard = JsonSerializer.Serialize(original);
        var deserializedCard = JsonSerializer.Deserialize<Card>(serializedCard);
        Assert.Equal(Rank.Four, original.Rank);
        Assert.Equal(Suit.Spades, original.Suit);
        Assert.Equal(original, deserializedCard);
    }
}