namespace Skunked;

/// <summary>
/// Event when the game has completed.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
public sealed record GameCompletedEvent(Guid GameId, int Version) : StreamEvent(GameId, Version);
