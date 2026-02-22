namespace Skunked;

/// <summary>
/// Result Calculated.
/// </summary>
/// <param name="Points">The set of points that can be scored.</param>
/// <param name="Combinations">The set of combinations that can score points.</param>
public sealed record CalculatedResult(CalculatedPoints Points, CalculatedCombinations Combinations);

/// <summary>
/// Set of points calculated.
/// </summary>
/// <param name="Score">Total points scored.</param>
/// <param name="FifteenScore">Points scored for fifteens.</param>
/// <param name="PairScore">Points scored for pairs.</param>
/// <param name="RunScore">Points scored for runs.</param>
/// <param name="FlushScore">Points scored for flushes.</param>
/// <param name="NobScore">Points scored for nobs.</param>
public sealed record CalculatedPoints(
    int Score,
    int FifteenScore,
    int PairScore,
    int RunScore,
    int FlushScore,
    int NobScore);

/// <summary>
/// A set of combinations that can score points.
/// </summary>
/// <param name="Fifteens">Sets of fifteens that can score points</param>
/// <param name="Pairs"> Sets of pairs that can score points</param>
/// <param name="Runs"> Sets of runs that can score points</param>
/// <param name="Flushes"> Sets of flushes that can score points</param>
/// <param name="Nobs"> Sets of nobs that can score points</param>
public record CalculatedCombinations(
    List<IReadOnlyList<Card>> Fifteens,
    List<IReadOnlyList<Card>> Pairs,
    List<IReadOnlyList<Card>> Runs,
    List<Card>? Flushes,
    Card? Nobs);
