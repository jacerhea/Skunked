using Xunit;

namespace Skunked.Test.System;

public class RunnerIsolationTests
{
    [Fact]
    public void PlayerCannotMutateRunnerStateThroughCallbackArguments()
    {
        var players = new[] { new MutatingPlayer(1), new MutatingPlayer(2) };
        var game = new GameRunner(new Deck(), () => new Random(7))
            .Run(players.Cast<IGameRunnerPlayer>().ToList(), WinningScore.Short61);
        Assert.True(game.State.IsGameFinished());
        Assert.All(players, p => { Assert.True(p.DiscardCalls > 0); Assert.True(p.HandCalls > 0); });
        Assert.True(players.Sum(p => p.CribCalls) > 0);
        Assert.All(game.State.Rounds, r => Assert.All(r.DealtCards, h => Assert.Equal(6, h.Hand.Count)));
    }

    [Fact]
    public void RunnerCopiesDeckAtConstruction()
    {
        var deck = new Deck();
        var runner = new GameRunner(deck, () => new Random(42));
        deck.Shuffle(new Random(100));
        var expected = new GameRunner(new Deck(), () => new Random(42));
        var a = runner.Run([new MutatingPlayer(1), new MutatingPlayer(2)], WinningScore.Short61);
        var b = expected.Run([new MutatingPlayer(1), new MutatingPlayer(2)], WinningScore.Short61);
        Assert.Equal(a.State.Rounds[0].DealtCards.SelectMany(h => h.Hand), b.State.Rounds[0].DealtCards.SelectMany(h => h.Hand));
    }

    private sealed class MutatingPlayer(int id) : IGameRunnerPlayer
    {
        private readonly ScoreCalculator scorer = new();
        public int Id => id;
        public int DiscardCalls, HandCalls, CribCalls;
        private static List<Card> CopyAndMutate(IEnumerable<Card> cards)
        {
            var copy = cards.ToList();
            if (cards is IList<Card> list && list.Count > 0) list[0] = new Card(Rank.Ace, Suit.Clubs);
            return copy;
        }
        public Card CutCards(IEnumerable<Card> cards) => cards.First();
        public List<Card> DetermineCardsToThrow(IEnumerable<Card> hand) { DiscardCalls++; return CopyAndMutate(hand).Take(2).ToList(); }
        public Card DetermineCardsToPlay(GameRules rules, IEnumerable<Card> pile, IEnumerable<Card> hand)
        {
            var count = scorer.SumValues(pile);
            return hand.First(c => count + scorer.SumValues([c]) <= GameRules.Points.MaxPlayCount);
        }
        public int CountHand(Card starter, IEnumerable<Card> hand) { HandCalls++; return scorer.CountShowPoints(starter, CopyAndMutate(hand)).Points.Score; }
        public int CountCrib(Card starter, IEnumerable<Card> crib) { CribCalls++; return scorer.CountShowPoints(starter, CopyAndMutate(crib), true).Points.Score; }
    }
}
