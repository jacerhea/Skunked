namespace Skunked;

/// <summary>
/// The base cribbage game event.
/// </summary>
public abstract class StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    protected StreamEvent(Guid gameId, int version)
    {
        GameId = gameId;
        Version = version;
    }

    /// <summary>
    /// Gets identifier of the game.
    /// </summary>
    public Guid GameId { get; }

    /// <summary>
    /// Gets version of the game state.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets the time stamp this event took place.
    /// </summary>
    public DateTimeOffset Occurred { get; } = DateTimeOffset.Now;
}