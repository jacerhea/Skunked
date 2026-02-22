namespace Skunked;

/// <summary>
/// Event when a round has started.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
public sealed record RoundStartedEvent(Guid GameId, int Version) : StreamEvent(GameId, Version);
