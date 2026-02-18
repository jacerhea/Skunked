using Skunked;
using Xunit;

namespace Skunked.UnitTest.Utility;

public sealed class GameStateExtensionsTests
{
    private static GameState BuildState(int team1Score = 0, WinningScore winningScore = WinningScore.Standard121) =>
        new()
        {
            PlayerIds = new List<int> { 1, 2 },
            GameRules = new GameRules(winningScore),
            TeamScores = new List<TeamScore>
            {
                new() { Players = new List<int> { 1 }, Score = team1Score },
                new() { Players = new List<int> { 2 }, Score = 0 }
            },
            OpeningRound = new OpeningRound { CutCards = new List<PlayerIdCard>() },
            Rounds = new List<RoundState>()
        };

    // ── IsGameFinished ─────────────────────────────────────────────────────

    [Fact]
    public void IsGameFinished_Returns_False_When_Score_Below_Winning()
    {
        var state = BuildState(team1Score: 120);
        Assert.False(state.IsGameFinished());
    }

    [Fact]
    public void IsGameFinished_Returns_True_When_Score_Equals_Winning()
    {
        var state = BuildState(team1Score: 121);
        Assert.True(state.IsGameFinished());
    }

    [Fact]
    public void IsGameFinished_Returns_True_When_Score_Exceeds_Winning()
    {
        var state = BuildState(team1Score: 130);
        Assert.True(state.IsGameFinished());
    }

    [Fact]
    public void IsGameFinished_Uses_Short_Game_WinningScore()
    {
        var state = BuildState(team1Score: 61, winningScore: WinningScore.Short61);
        Assert.True(state.IsGameFinished());
    }

    // ── GetCurrentRound ────────────────────────────────────────────────────

    [Fact]
    public void GetCurrentRound_Returns_Round_With_Highest_Number()
    {
        var state = BuildState();
        state.Rounds = new List<RoundState>
        {
            new() { Round = 1 },
            new() { Round = 3 },
            new() { Round = 2 }
        };

        var current = state.GetCurrentRound();

        Assert.Equal(3, current.Round);
    }

    // ── GetNextPlayerFrom ──────────────────────────────────────────────────

    [Fact]
    public void GetNextPlayerFrom_Returns_Next_Player_In_Rotation()
    {
        var state = BuildState();
        Assert.Equal(2, state.GetNextPlayerFrom(1));
    }

    [Fact]
    public void GetNextPlayerFrom_Wraps_Around_To_First_Player()
    {
        var state = BuildState();
        Assert.Equal(1, state.GetNextPlayerFrom(2));
    }
}
