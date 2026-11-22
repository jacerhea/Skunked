using Xunit;
namespace Skunked.UnitTest;
public class ListenerConcurrencyTests
{
    [Fact] public void FailingListenerDoesNotInterruptCommandOrOtherListeners()
    {
        var good=new Collector(); var game=new Cribbage([1,2],new(),[new Failing(),good]);
        game.CutCard(new(1,game.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Ace)));
        game.CutCard(new(2,game.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Two)));
        Assert.Single(game.State.Rounds); Assert.Equal(game.Stream.Count(),good.Count);
        Assert.Equal(game.Stream.Count(),game.ListenerFailures.Count);
    }
    [Fact] public void ConcurrentCommandsAreSerialized()
    {
        var game=new Cribbage([1,2],new()); var card=game.State.OpeningRound.Deck.First();
        var successes=0;
        Parallel.For(0,16,_=> { try { game.CutCard(new(1,card)); Interlocked.Increment(ref successes); } catch (InvalidCribbageOperationException) {} });
        Assert.Equal(1,successes); Assert.Single(game.State.OpeningRound.CutCards);
    }
    [Fact] public void ConcurrentStreamEnumerationReturnsDetachedPayloads()
    {
        var game = new Cribbage([1,2],new());
        var started = Assert.IsType<GameStartedEvent>(game.Stream.First());
        started.Players.Clear();
        Assert.Equal(2, Assert.IsType<GameStartedEvent>(game.Stream.First()).Players.Count);
        Assert.Equal(2, game.State.PlayerIds.Count);
    }
    private sealed class Failing : IEventListener { public void Notify(StreamEvent e)=>throw new InvalidOperationException("listener failed"); }
    private sealed class Collector : IEventListener { public int Count; public void Notify(StreamEvent e)=>Count++; }
}
