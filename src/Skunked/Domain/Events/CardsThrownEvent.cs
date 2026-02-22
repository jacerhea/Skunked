namespace Skunked;

/// <summary>
/// Event when cards have been thrown to the crib.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="PlayerId">The id of the player.</param>
/// <param name="Thrown">The cards thrown to the crib.</param>
public sealed record CardsThrownEvent(Guid GameId, int Version, int PlayerId, List<Card> Thrown)
    : StreamEvent(GameId, Version);
