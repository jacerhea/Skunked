namespace Skunked;

/// <summary>
/// Represents a starter card and a score.
/// </summary>
/// <param name="Score">Gets the score.</param>
/// <param name="Cut">Gets the cut card.</param>
public sealed record ScoreWithCut(int Score, Card Cut);