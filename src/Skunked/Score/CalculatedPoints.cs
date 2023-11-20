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

    /// <summary>
    /// The set of points that can be scored.
    /// </summary>
    public CalculatedPoints Points { get; }

    /// <summary>
    /// The set of combinations that can score points.
    /// </summary>
    public CalculatedCombinations Combinations { get; }
}

/// <summary>
/// Set of points calculated.
/// </summary>
public class CalculatedPoints
{
    /// <summary>
    /// Total points scored.
    /// </summary>
    public int Score { get; }

    /// <summary>
    /// Points scored for fifteens. 
    /// </summary>
    public int FifteenScore { get; }

    /// <summary>
    /// Points scored for pairs.
    /// </summary>
    public int PairScore { get; }

    /// <summary>
    /// Points scored for runs.
    /// </summary>
    public int RunScore { get; }

    /// <summary>
    /// Points scored for flushes.
    /// </summary>
    public int FlushScore { get; }

    /// <summary>
    /// Points scored for nobs.
    /// </summary>
    public int NobScore { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    /// <param name="fifteenScore"></param>
    /// <param name="pairScore"></param>
    /// <param name="runScore"></param>
    /// <param name="flushScore"></param>
    /// <param name="nobScore"></param>
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
/// <param name="Fifteens">Sets of fifteens that can score points</param>
/// <param name="Pairs"> Sets of pairs that can score points</param>
/// <param name="Runs"> Sets of runs that can score points</param>
/// <param name="Flushes"> Sets of flushes that can score points</param>
/// <param name="Nobs"> Sets of nobs that can score points</param>
public record CalculatedCombinations(IList<IReadOnlyList<Card>> Fifteens,
    IList<IReadOnlyList<Card>> Pairs,
    IList<IReadOnlyList<Card>> Runs,
    IList<Card> Flushes,
    Card? Nobs);