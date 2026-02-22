namespace Skunked;

/// <summary>
/// Event when the play phase has started.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="Round">The round number.</param>
public sealed record PlayStartedEvent(Guid GameId, int Version, int Round)
    : StreamEvent(GameId, Version);
