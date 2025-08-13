namespace Skunked.Domain.Events;

/// <summary>
/// Event when the play has started.
/// </summary>
public sealed class PlayStartedEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayStartedEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="round">The round.</param>
    public PlayStartedEvent(Guid gameId, int version, int round)
        : base(gameId, version)
    {
        Round = round;
    }

    /// <summary>
    /// The round.
    /// </summary>
    public int Round { get; }
}