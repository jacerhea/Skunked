using Skunked.Cards;

namespace Skunked.Score;

/// <summary>
/// Represents a starter card and a score.
/// </summary>
public sealed class ScoreWithCut
{
    /// <summary>
    /// Gets the score.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Gets the cut card.
    /// </summary>
    public Card Cut { get; set; }
}