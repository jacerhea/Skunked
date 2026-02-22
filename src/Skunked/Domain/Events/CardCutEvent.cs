namespace Skunked;

/// <summary>
/// Event when a card has been cut.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="PlayerId">The id of the player who cut the card.</param>
/// <param name="CutCard">The card that was cut.</param>
public sealed record CardCutEvent(Guid GameId, int Version, int PlayerId, Card CutCard)
    : StreamEvent(GameId, Version);
