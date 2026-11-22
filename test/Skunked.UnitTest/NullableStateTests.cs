using Xunit;
namespace Skunked.UnitTest;
public class NullableStateTests
{
    [Fact] public void RoundStarterIsAbsentUntilSelected()
    {
        var game = new Cribbage([1,2], new());
        game.CutCard(new(1,game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Ace)));
        game.CutCard(new(2,game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Two)));
        Assert.Null(game.State.GetCurrentRound().Starter);
        foreach(var id in game.State.PlayerIds)
            game.ThrowCards(new(id,game.State.GetCurrentRound().DealtCards.Single(h=>h.PlayerId==id).Hand.Take(2)));
        Assert.NotNull(game.State.GetCurrentRound().Starter);
    }
    [Fact] public void MissingRoundFailsClearly() => Assert.Throws<InvalidOperationException>(()=>new GameState().GetCurrentRound());
}
