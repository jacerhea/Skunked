namespace Skunked;

/// <summary>
/// The base cribbage game event.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
public abstract record StreamEvent(Guid GameId, int Version)
{
    /// <summary>
    /// Gets the time stamp this event took place.
    /// </summary>
    public DateTimeOffset Occurred { get; } = DateTimeOffset.Now;
}
