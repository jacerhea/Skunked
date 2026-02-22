namespace Skunked;

/// <summary>
/// Represents a starter card and a score.
/// </summary>
public sealed record ScoreWithCut
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