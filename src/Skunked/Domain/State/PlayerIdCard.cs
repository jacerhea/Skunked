namespace Skunked;

/// <summary>
/// A player id and a card.
/// </summary>
/// <param name="Player">The player identifier.</param>
/// <param name="Card">The card associated with the player.</param>
public sealed record PlayerIdCard(int Player, Card Card);
