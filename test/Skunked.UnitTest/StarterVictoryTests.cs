using Xunit;
namespace Skunked.UnitTest;
public class StarterVictoryTests
{
    [Fact] public void WinningOnJackStarterEndsBeforePlayStarts()
    {
        for (var seed=0; seed<100; seed++)
        {
            var game=new Cribbage([1,2],new GameRules(WinningScore.Short61),new Random(seed),null);
            game.CutCard(new(1,game.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Ace)));
            game.CutCard(new(2,game.State.OpeningRound.Deck.First(c=>c.Rank==Rank.Two)));
            var round=game.LiveState.GetCurrentRound();
            var dealer=round.PlayerCrib;
            game.LiveState.TeamScores.Single(t=>t.Players.Contains(dealer)).Score=60;
            foreach(var id in game.State.PlayerIds)
                game.ThrowCards(new(id,game.State.GetCurrentRound().DealtCards.Single(h=>h.PlayerId==id).Hand.Take(2)));
            if (game.State.GetCurrentRound().Starter.Rank != Rank.Jack) continue;
            Assert.IsType<GameCompletedEvent>(game.Stream.Last());
            Assert.DoesNotContain(game.Stream, e => e is PlayStartedEvent);
            Assert.NotNull(game.State.CompletedAt);
            return;
        }
        throw new Xunit.Sdk.XunitException("No tested seed selected a jack starter.");
    }
}
