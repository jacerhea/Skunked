using System.Collections.Generic;
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

public record CalculatedPoints(
    int Score,
    int FifteenScore,
    int PairScore,
    int RunScore,
    int FlushScore,
    int NobScore);

/// <summary>
/// A set of combinations that can score points.
/// </summary>
public record CalculatedCombinations(
    List<List<Card>> Fifteens,
    List<List<Card>> Pairs,
    List<List<Card>> Runs,
    List<Card> Flushes,
    Card? Nobs);