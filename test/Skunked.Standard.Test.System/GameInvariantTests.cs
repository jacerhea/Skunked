using System.Text.Json;
using Xunit;

namespace Skunked.Test.System;

public sealed class GameInvariantTests
{
    [Theory]
    [InlineData(2, 3)] [InlineData(2, 27)] [InlineData(4, 3)] [InlineData(4, 27)]
    public void SeededGamesPreserveCardsScoresAndReplay(int playerCount, int seed)
    {
        var runner = new GameRunner(new Deck(), () => new Random(seed));
        var players = Enumerable.Range(1, playerCount).Select(id => (IGameRunnerPlayer)new DeterministicPlayer(id)).ToList();
        var game = runner.Run(players, WinningScore.Short61);
        var state = game.State;
        var events = game.Stream.ToList();

        Assert.True(state.IsGameFinished());
        Assert.NotNull(state.CompletedAt);
        Assert.Equal(Enumerable.Range(1, events.Count), events.Select(e => e.Version));
        Assert.All(events, e => Assert.Equal(state.Id, e.GameId));
        foreach (var team in state.TeamScores)
            Assert.Equal(team.Score, state.IndividualScores.Where(s => team.Players.Contains(s.Player)).Sum(s => s.Score));

        foreach (var round in state.Rounds)
        {
            var dealt = round.DealtCards.SelectMany(h => h.Hand).ToList();
            Assert.Equal(playerCount * state.GameRules.GetDealSize(playerCount), dealt.Count);
            Assert.Equal(dealt.Count, dealt.Distinct().Count());
            Assert.All(round.DealtCards, h => Assert.Equal(state.GameRules.GetDealSize(playerCount), h.Hand.Count));
            var played = round.ThePlay.SelectMany(p => p).Select(p => p.Card).ToList();
            Assert.Equal(played.Count, played.Distinct().Count());
            Assert.All(played, card => Assert.Contains(card, dealt));
            if (round.ThrowCardsComplete)
            {
                Assert.Equal(4, round.Crib.Count);
                Assert.All(round.Hands, h => Assert.Equal(4, h.Hand.Count));
                Assert.Equal(dealt.Count, round.Hands.SelectMany(h => h.Hand).Concat(round.Crib).Distinct().Count());
                Assert.DoesNotContain(round.Starter, dealt);
            }
        }

        var reconstructed = GameStateBuilder.Replay(events);
        Assert.Equal(JsonSerializer.Serialize(state), JsonSerializer.Serialize(reconstructed));
    }

    private sealed class DeterministicPlayer(int id) : IGameRunnerPlayer
    {
        public int Id => id;
        public Card CutCards(IEnumerable<Card> cardsToChoose) => cardsToChoose.First();
        public List<Card> DetermineCardsToThrow(IEnumerable<Card> hand) => hand.Take(hand.Count() - GameRules.HandSize).ToList();
        public Card DetermineCardsToPlay(GameRules rules, IEnumerable<Card> pile, IEnumerable<Card> handLeft)
        {
            var scorer = new ScoreCalculator();
            var count = scorer.SumValues(pile);
            return handLeft.First(c => count + scorer.SumValues([c]) <= GameRules.Points.MaxPlayCount);
        }
        public int CountHand(Card starter, IEnumerable<Card> hand) => new ScoreCalculator().CountShowPoints(starter, hand).Points.Score;
    }
}
