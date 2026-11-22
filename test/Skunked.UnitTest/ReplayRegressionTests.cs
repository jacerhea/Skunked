using Xunit;
namespace Skunked.UnitTest;
public class ReplayRegressionTests
{
    [Fact] public void ReplaysGameAndUsesLastEventTimestamp()
    {
        var g=new Cribbage([1,2],new());
        g.CutCard(new(1,g.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Ace)));
        g.CutCard(new(2,g.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Two)));
        foreach(var id in g.State.PlayerIds) g.ThrowCards(new(id,g.State.GetCurrentRound().DealtCards.Single(h=>h.PlayerId==id).Hand.Take(2)));
        var replay=GameStateBuilder.Replay(g.Stream);
        Assert.Equal(g.State.Id,replay.Id); Assert.Equal(g.State.Version,replay.Version);
        Assert.Equal(g.Stream.Last().Occurred,replay.LastUpdated); Assert.Equal(g.State.LastUpdated,replay.LastUpdated);
        Assert.Equal(g.State.GetCurrentRound().Starter,replay.GetCurrentRound().Starter);
        Assert.Equal(g.State.GetCurrentRound().Crib,replay.GetCurrentRound().Crib);
        Assert.Equal(g.State.TeamScores.Select(t=>t.Score),replay.TeamScores.Select(t=>t.Score));
    }
    [Fact] public void RejectsEmptyMissingAndForeignEvents()
    {
        Assert.Throws<InvalidOperationException>(()=>GameStateBuilder.Replay([]));
        var id=Guid.NewGuid(); StreamEvent start=new GameStartedEvent(id,1,[1,2],new());
        Assert.Throws<InvalidOperationException>(()=>GameStateBuilder.Replay([start,new RoundStartedEvent(id,3)]));
        Assert.Throws<InvalidOperationException>(()=>GameStateBuilder.Replay([start,new RoundStartedEvent(Guid.NewGuid(),2)]));
    }
}
