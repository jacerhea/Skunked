using Xunit;
namespace Skunked.UnitTest;
public class StateOwnershipTests
{
    [Fact] public void ChangingSnapshotOrEventPayloadDoesNotChangeGame()
    {
        var game=new Cribbage([1,2],new());
        var snapshot=game.State; snapshot.PlayerIds.Clear(); snapshot.OpeningRound.Deck.Clear();
        var started=Assert.IsType<GameStartedEvent>(game.Stream.First()); started.Players.Clear();
        var shuffled=Assert.IsType<DeckShuffledEvent>(game.Stream.Skip(1).First()); shuffled.Deck.Clear();
        Assert.Equal(2,game.State.PlayerIds.Count); Assert.Equal(52,game.State.OpeningRound.Deck.Count);
        Assert.Equal(2,Assert.IsType<GameStartedEvent>(game.Stream.First()).Players.Count);
        Assert.Equal(52,Assert.IsType<DeckShuffledEvent>(game.Stream.Skip(1).First()).Deck.Count);
    }
    [Fact] public void EventListenersCannotChangeHistoryOrInternalState()
    {
        var game=new Cribbage([1,2],new(),[new Mutator()]);
        Assert.Equal(2,game.State.PlayerIds.Count); Assert.Equal(52,game.State.OpeningRound.Deck.Count);
        Assert.Equal(2,Assert.IsType<GameStartedEvent>(game.Stream.First()).Players.Count);
    }
    private sealed class Mutator : IEventListener
    {
        public void Notify(StreamEvent e) { if(e is GameStartedEvent s) s.Players.Clear(); if(e is DeckShuffledEvent d) d.Deck.Clear(); }
    }
}
