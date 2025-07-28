using Skunked.Cards;

namespace Skunked.Domain.State;

/// <summary>
/// State of the opening round
/// </summary>
public class OpeningRound
{
    /// <summary>
    /// State of the deck
    /// </summary>
    public List<Card> Deck { get; set; } = new();

    /// <summary>
    /// State of the cut cards.
    /// </summary>
    public List<PlayerIdCard> CutCards { get; set; } = new();

    /// <summary>
    /// Is the opening round complete.
    /// </summary>
    public bool Complete { get; set; }

    /// <summary>
    /// The index of the winning player cut.
    /// </summary>
    public int? WinningPlayerCut { get; set; }
}