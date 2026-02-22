namespace Skunked;

/// <summary>
/// Event when all hands have been dealt.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="Hands">All player hands that were dealt.</param>
public sealed record HandsDealtEvent(Guid GameId, int Version, List<PlayerHand> Hands)
    : StreamEvent(GameId, Version);
