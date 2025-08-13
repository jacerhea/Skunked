namespace Skunked.Domain.Events;

/// <summary>
/// Event when a hand has been counted.
/// </summary>
public sealed class HandCountedEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HandCountedEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="playerId">The id of the player.</param>
    /// <param name="countedScore">Gets the counted score.</param>
    public HandCountedEvent(Guid gameId, int version, int playerId, int countedScore)
        : base(gameId, version)
    {
        PlayerId = playerId;
        CountedScore = countedScore;
    }

    /// <summary>
    /// Gets the player id.
    /// </summary>
    public int PlayerId { get; }

    /// <summary>
    /// Gets the counted score.
    /// </summary>
    public int CountedScore { get; }
}