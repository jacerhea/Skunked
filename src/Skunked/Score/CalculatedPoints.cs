using Skunked.Cards;

namespace Skunked.Score;

/// <summary>
/// Result Calculated.
/// </summary>
public class CalculatedResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CalculatedResult"/> class.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="combinations"></param>
    public CalculatedResult(CalculatedPoints points, CalculatedCombinations combinations)
    {
        Points = points;
        Combinations = combinations;
    }

    public CalculatedPoints Points { get; }
    public CalculatedCombinations Combinations { get; }
}

/// <summary>
/// Set of points calculated.
/// </summary>
public class CalculatedPoints
{
    public int Score { get; }
    public int FifteenScore { get; }
    public int PairScore { get; }
    public int RunScore { get; }
    public int FlushScore { get; }
    public int NobScore { get; }

    public CalculatedPoints(int score,
        int fifteenScore,
        int pairScore,
        int runScore,
        int flushScore,
        int nobScore)
    {
        Score = score;
        FifteenScore = fifteenScore;
        PairScore = pairScore;
        RunScore = runScore;
        FlushScore = flushScore;
        NobScore = nobScore;
    }
}

/// <summary>
/// A set of combinations that can score points.
/// </summary>
public record CalculatedCombinations(IList<IList<Card>> Fifteens,
    IList<IList<Card>> Pairs,
    IList<IList<Card>> Runs,
    IList<Card> Flushes,
    Card? Nobs);