namespace Skunked;

/// <summary>
/// Event when the deck has been shuffled.
/// </summary>
/// <param name="GameId">Unique identifier of the game.</param>
/// <param name="Version">The version of the game.</param>
/// <param name="Deck">The new state of the deck after the shuffle.</param>
public sealed record DeckShuffledEvent(Guid GameId, int Version, List<Card> Deck)
    : StreamEvent(GameId, Version);
