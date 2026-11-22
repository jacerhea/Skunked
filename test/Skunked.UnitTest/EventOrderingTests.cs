using Xunit;
namespace Skunked.UnitTest;
public class EventOrderingTests
{
    [Fact] public void GameProducesConsecutiveVersionsIncludingCuts()
    {
        var game = new Cribbage([1,2], new());
        game.CutCard(new(1, game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Ace)));
        game.CutCard(new(2, game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Two)));
        Assert.Equal(Enumerable.Range(1, game.Stream.Count()), game.Stream.Select(e => e.Version));
    }
    [Fact] public void StreamRejectsWrongVersionAndGameWithoutNotifying()
    {
        var id=Guid.NewGuid(); var listener=new Collector(); var stream=new EventStream([listener]);
        stream.Add(new GameStartedEvent(id, 1, [1,2], new()));
        Assert.Throws<InvalidOperationException>(()=>stream.Add(new RoundStartedEvent(id, 3)));
        Assert.Throws<InvalidOperationException>(()=>stream.Add(new RoundStartedEvent(Guid.NewGuid(), 2)));
        Assert.Single(stream); Assert.Single(listener.Events);
        stream.Add(new RoundStartedEvent(id, 2)); Assert.Equal(2,stream.Count());
    }
    private sealed class Collector : IEventListener { public List<StreamEvent> Events { get; }=[]; public void Notify(StreamEvent e)=>Events.Add(e); }
}
