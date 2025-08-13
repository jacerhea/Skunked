using Skunked.Cards;

namespace Skunked.Domain.State;

/// <summary>
/// The state of the pre-round.
/// </summary>
public sealed class PreRound
{
    /// <summary>
    /// The state of the deck before the round begins.
    /// </summary>
    public List<Card> Deck { get; set; } = new();
}