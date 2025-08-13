namespace Skunked.Domain.State;

/// <summary>
/// Snapshot of a game's state.
/// </summary>
public sealed class GameState
{
    /// <summary>
    /// Gets or sets unique identifier of the game.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets version of the game.  Each event creates a new version of the game.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets player ids.
    /// </summary>
    public List<int> PlayerIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the players scores.
    /// </summary>
    public List<PlayerScore> IndividualScores { get; set; } = new();

    /// <summary>
    /// Gets or sets the team scores.
    /// </summary>
    public List<TeamScore> TeamScores { get; set; } = new();

    /// <summary>
    /// Gets or sets the set of rules for the game.
    /// </summary>
    public GameRules GameRules { get; set; } = null!;

    /// <summary>
    /// Gets or set the opening round.
    /// </summary>
    public OpeningRound OpeningRound { get; set; } = null!;

    /// <summary>
    /// Gets or set the set of rounds.
    /// </summary>
    public List<RoundState> Rounds { get; set; } = new();

    /// <summary>
    /// Gets or sets the time stamp of when the game started.
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the time stamp of when the last event occurred.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the time stamp of when the game we completed.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }
}