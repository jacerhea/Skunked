using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Score
{
    /// <summary>
    /// Result Calculated.
    /// </summary>
    public class CalculatedResult
    {
        public CalculatedResult(CalculatedPoints points, CalculatedCombinations combinations)
        {
            Points = points;
            Combinations = combinations;
        }

        public CalculatedPoints Points { get; }
        public CalculatedCombinations Combinations { get; }
    }

    /// <summary>
    /// Set of points scored.
    /// </summary>
    public record CalculatedPoints(int Score,
        int FifteenScore,
        int PairScore,
        int RunScore,
        int FlushScore,
        int NobScore);

    /// <summary>
    /// A set of combinations that can score points.
    /// </summary>
    public record CalculatedCombinations(IList<IList<Card>> Fifteens,
        IList<IList<Card>> Pairs,
        IList<IList<Card>> Runs,
        IList<Card> Flushes,
        IList<Card> Nobs);
}
