namespace Skunked;

/// <summary>
/// Event when a hand has been counted.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="PlayerId">The id of the player.</param>
/// <param name="CountedScore">The score the player declared for their hand.</param>
public sealed record HandCountedEvent(Guid GameId, int Version, int PlayerId, int CountedScore)
    : StreamEvent(GameId, Version);
