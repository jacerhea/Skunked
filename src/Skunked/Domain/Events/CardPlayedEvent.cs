namespace Skunked;

/// <summary>
/// Event when a card has been played.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="PlayerId">The id of the player.</param>
/// <param name="Played">The card played.</param>
public sealed record CardPlayedEvent(Guid GameId, int Version, int PlayerId, Card Played)
    : StreamEvent(GameId, Version);
