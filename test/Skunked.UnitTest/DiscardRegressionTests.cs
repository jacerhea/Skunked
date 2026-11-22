using Xunit;
namespace Skunked.UnitTest;
public class DiscardRegressionTests
{
    [Theory]
    [InlineData(0)] [InlineData(1)] [InlineData(3)]
    public void RejectsWrongCount(int count)
    {
        var game = Game(); var round = game.State.GetCurrentRound();
        var before = game.Stream.Count();
        Assert.Throws<InvalidCribbageOperationException>(() => game.ThrowCards(new(1, round.DealtCards.Single(h => h.PlayerId == 1).Hand.Take(count))));
        Assert.Equal(before, game.Stream.Count());
    }
    [Fact] public void RejectsRepeatedDiscard()
    {
        var game = Game(); var cards = game.State.GetCurrentRound().DealtCards.Single(h => h.PlayerId == 1).Hand;
        game.ThrowCards(new(1, cards.Take(2))); var before = game.Stream.Count();
        Assert.Throws<InvalidCribbageOperationException>(() => game.ThrowCards(new(1, cards.Skip(2).Take(2))));
        Assert.Equal(before, game.Stream.Count());
    }
    [Fact] public void RejectsDuplicateCards()
    {
        var game = Game(); var card = game.State.GetCurrentRound().DealtCards.Single(h => h.PlayerId == 1).Hand[0];
        Assert.Throws<InvalidCribbageOperationException>(() => game.ThrowCards(new(1, [card, card])));
    }
    private static Cribbage Game()
    {
        var game = new Cribbage([1, 2], new());
        game.CutCard(new(1, game.State.OpeningRound.Deck.Single(c => c.Rank == Rank.Ace && c.Suit == Suit.Clubs)));
        game.CutCard(new(2, game.State.OpeningRound.Deck.Single(c => c.Rank == Rank.Two && c.Suit == Suit.Clubs)));
        return game;
    }
}
