namespace Skunked;

/// <summary>
/// The state of the pre-round.
/// </summary>
public sealed record PreRound
{
    /// <summary>
    /// The state of the deck before the round begins.
    /// </summary>
    public List<Card> Deck { get; set; } = [];
}