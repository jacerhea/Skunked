namespace Skunked;

/// <summary>
/// A player id and their hand.
/// </summary>
public sealed class PlayerHand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerHand"/> class.
    /// </summary>
    /// <param name="playerId">The id of the player.</param>
    /// <param name="hand">The players hand.</param>
    public PlayerHand(int playerId, List<Card> hand)
    {
        ArgumentNullException.ThrowIfNull(hand, nameof(hand));
        PlayerId = playerId;
        Hand = hand;
    }

    /// <summary>
    /// Gets the id of the player.
    /// </summary>
    public int PlayerId { get; }

    /// <summary>
    /// Gets the hand of the player.
    /// </summary>
    public List<Card> Hand { get; }
}