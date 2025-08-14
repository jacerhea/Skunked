namespace Skunked;

/// <summary>
/// Event when all hands have been dealt.
/// </summary>
public sealed class HandsDealtEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HandsDealtEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="hands">All player hands have been dealt.</param>
    public HandsDealtEvent(Guid gameId, int version, List<PlayerHand> hands)
        : base(gameId, version)
    {
        Hands = hands;
    }

    /// <summary>
    /// All of the player hands that were dealt.
    /// </summary>
    public List<PlayerHand> Hands { get; }
}