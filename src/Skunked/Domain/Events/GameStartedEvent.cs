namespace Skunked;

/// <summary>
/// Event when the game has started.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="Players">The players in turn order.</param>
/// <param name="Rules">The set of rules for the game.</param>
public sealed record GameStartedEvent(Guid GameId, int Version, List<int> Players, GameRules Rules)
    : StreamEvent(GameId, Version);
