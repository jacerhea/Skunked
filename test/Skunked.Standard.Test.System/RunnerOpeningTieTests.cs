using Xunit;

namespace Skunked.Test.System;

public class RunnerOpeningTieTests
{
    [Fact]
    public void RunnerRecutsTiesAndReturnsCompletedGame()
    {
        var first = new Player(1);
        var second = new Player(2);
        var game = new GameRunner(new Deck(), () => new Random(23)).Run([first, second], WinningScore.Short61);
        Assert.Equal(2, first.Cuts);
        Assert.Equal(2, second.Cuts);
        Assert.True(game.State.IsGameFinished());
        Assert.NotNull(game.State.CompletedAt);
        Assert.IsType<GameCompletedEvent>(game.Stream.Last());
    }

    private sealed class Player(int id) : IGameRunnerPlayer
    {
        private readonly ScoreCalculator scorer = new();
        public int Id => id;
        public int Cuts { get; private set; }
        public Card CutCards(IEnumerable<Card> cards)
        {
            Cuts++;
            var rank = Cuts == 1 ? Rank.Ace : id == 1 ? Rank.Two : Rank.Three;
            return cards.First(c => c.Rank == rank);
        }
        public List<Card> DetermineCardsToThrow(IEnumerable<Card> hand) => hand.Take(2).ToList();
        public Card DetermineCardsToPlay(GameRules rules, IEnumerable<Card> pile, IEnumerable<Card> hand)
        {
            var count = scorer.SumValues(pile);
            return hand.First(card => count + scorer.SumValues([card]) <= GameRules.Points.MaxPlayCount);
        }
        public int CountHand(Card starter, IEnumerable<Card> hand) => scorer.CountShowPoints(starter, hand).Points.Score;
    }
}
