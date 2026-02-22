namespace Skunked;

/// <summary>
/// Event when a starter card has been selected.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="Starter">The starter card that was cut.</param>
public sealed record StarterCardSelectedEvent(Guid GameId, int Version, Card Starter)
    : StreamEvent(GameId, Version);
