using Xunit;
namespace Skunked.UnitTest;
public class CribScoringRegressionTests
{
    [Theory] [InlineData(Suit.Hearts, 0)] [InlineData(Suit.Clubs, 5)]
    public void CribFlushUsesStarterSuit(Suit starterSuit, int flushScore)
    {
        var starter = new Card(Rank.King, starterSuit);
        var crib = new List<Card> { new(Rank.Ace, Suit.Clubs), new(Rank.Two, Suit.Clubs), new(Rank.Three, Suit.Clubs), new(Rank.Four, Suit.Clubs) };
        var state = new GameState { PlayerIds = [1,2], GameRules = new(), TeamScores = [new() { Players = [1] },new() { Players = [2] }], IndividualScores = [new() { Player = 1 },new() { Player = 2 }], Rounds = [new() { Round=1, PlayerCrib=1, Starter=starter, Crib=crib, ShowScores=[new() { Player=1 },new() { Player=2 }] }] };
        var scorer = new ScoreCalculator(); var correct = scorer.CountShowPoints(starter, crib, true);
        Assert.Equal(flushScore, correct.Points.FlushScore);
        new GameStateBuilder(state).Notify(new CribCountedEvent(Guid.NewGuid(), 1, 1, correct.Points.Score));
        Assert.Equal(correct.Points.Score, state.TeamScores[0].Score);
        Assert.Equal(correct.Points.Score, state.Rounds[0].ShowScores[0].CribScore);
    }
}

