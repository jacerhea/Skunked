namespace Skunked;

/// <summary>
/// A player id and a card.
/// </summary>
public sealed class PlayerIdCard
{
    /// <summary>
    /// Gets the playerId.
    /// </summary>
    public int Player { get; init; }

    /// <summary>
    /// Gets the card.
    /// </summary>
    public Card Card { get; init; }
}