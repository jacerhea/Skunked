using Xunit;
namespace Skunked.UnitTest;
public class RandomInjectionTests
{
    [Fact] public void SeedReproducesShuffleDealAndStarter()
    {
        var a=Create(42); var b=Create(42);
        Assert.Equal(a.State.GetCurrentRound().PreRound!.Deck, b.State.GetCurrentRound().PreRound!.Deck);
        Assert.Equal(a.State.GetCurrentRound().Starter, b.State.GetCurrentRound().Starter);
        Assert.Equal(a.State.GetCurrentRound().DealtCards.SelectMany(x=>x.Hand), b.State.GetCurrentRound().DealtCards.SelectMany(x=>x.Hand));
    }
    [Fact] public void RejectsIncompleteDeck() => Assert.Throws<ArgumentException>(() => new Cribbage([1,2],new(),new Random(1),deck:new Deck([])));
    private static Cribbage Create(int seed)
    {
        var g=new Cribbage([1,2],new(),random:new Random(seed),deck:null);
        g.CutCard(new(1,g.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Ace)));
        g.CutCard(new(2,g.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Two)));
        foreach(var id in g.State.PlayerIds) g.ThrowCards(new(id,g.State.GetCurrentRound().DealtCards.Single(h=>h.PlayerId==id).Hand.Take(2)));
        return g;
    }
}
