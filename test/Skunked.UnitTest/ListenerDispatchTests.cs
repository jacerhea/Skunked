using System.Collections.Concurrent;
using Xunit;

namespace Skunked.UnitTest;

public class ListenerDispatchTests
{
    [Fact]
    public void ListenerCanWaitForAnotherThreadToReadAndIssueCommand()
    {
        var listener = new CrossThreadListener();
        var game = new Cribbage([1, 2], new(), [listener]);
        listener.Game = game;
        var first = game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Ace);
        game.CutCard(new(1, first));
        Assert.True(listener.Completed);
        Assert.Empty(game.ListenerFailures);
        Assert.Single(game.State.Rounds);
        Assert.Equal(game.Stream.Select(e => e.Version), listener.Versions);
    }

    private sealed class CrossThreadListener : IEventListener
    {
        public Cribbage? Game;
        public bool Completed;
        public List<int> Versions { get; } = [];
        public void Notify(StreamEvent item)
        {
            Versions.Add(item.Version);
            if (item is not CardCutEvent || Game is null || Completed) return;
            var game = Game;
            var work = Task.Run(() =>
            {
                var state = game.State;
                _ = game.Stream.ToList();
                var card = state.OpeningRound.Deck.First(c => c.Rank == Rank.Two);
                game.CutCard(new(2, card));
            });
            if (!work.Wait(TimeSpan.FromSeconds(3))) throw new TimeoutException("Listener blocked game access.");
            Completed = true;
        }
    }

    [Fact]
    public async Task SlowListenerDoesNotBlockConcurrentReadersOrCommands()
    {
        var listener = new BlockingListener();
        var game = new Cribbage([1, 2], new(), [listener]);
        listener.Enabled = true;
        var first = game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Ace);
        var command = Task.Run(() => game.CutCard(new(1, first)));
        try
        {
            Assert.True(listener.Entered.Wait(TimeSpan.FromSeconds(3)));
            var read = Task.Run(() => (game.State.Version, game.Stream.Count()));
            await read.WaitAsync(TimeSpan.FromSeconds(3));
            var second = game.State.OpeningRound.Deck.First(c => c.Rank == Rank.Two);
            var next = Task.Run(() => game.CutCard(new(2, second)));
            await next.WaitAsync(TimeSpan.FromSeconds(3));
        }
        finally { listener.Release.Set(); }
        await command.WaitAsync(TimeSpan.FromSeconds(3));
        Assert.Equal(game.Stream.Select(e => e.Version), listener.Versions);
    }

    private sealed class BlockingListener : IEventListener
    {
        public bool Enabled;
        public ManualResetEventSlim Entered = new(false), Release = new(false);
        public List<int> Versions { get; } = [];
        public void Notify(StreamEvent item)
        {
            Versions.Add(item.Version);
            if (!Enabled || item is not CardCutEvent || Entered.IsSet) return;
            Entered.Set();
            if (!Release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
        }
    }
}
