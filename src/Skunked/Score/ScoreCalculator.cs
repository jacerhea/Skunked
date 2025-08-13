using Combinatorics.Collections;
using Skunked.Cards;
using Skunked.Cards.Value;

namespace Skunked.Score;

/// <summary>
/// Calculates points scored in shows and plays.
/// </summary>
public sealed class ScoreCalculator
{
    private static readonly AceLowFaceTenCardValueStrategy ValueStrategy = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScoreCalculator"/> class.
    /// </summary>
    public ScoreCalculator()
    {
    }

    /// <summary>
    /// Check cut card for dealer.
    /// </summary>
    /// <param name="cut">The cut card.</param>
    /// <returns>Points to the dealer for the cut card.</returns>
    public int CountCut(Card cut) => cut.Rank == Rank.Jack ? GameRules.Points.Nibs : 0;

    /// <summary>
    /// Calculate points scored in a show.
    /// </summary>
    /// <param name="starterCard">The starter card.</param>
    /// <param name="playerHand">Cards in the players hand.</param>
    /// <param name="isCrib">A crib is being scored.</param>
    /// <returns>The calculated result of points earned.</returns>
    public CalculatedResult CountShowPoints(Card starterCard, IEnumerable<Card> playerHand, bool isCrib = false)
    {
        var playerHandList = playerHand.ToList();
        var completeSet = playerHandList.Append(starterCard).ToList();
        var allCombinations = GetCombinations(completeSet);

        var fifteens = FindFifteens(allCombinations);
        var flush = FindFlush(playerHandList, starterCard);
        var pairs = FindPairs(allCombinations);
        var runs = FindRuns(allCombinations);
        var hisNobs = FindNobs(playerHandList, starterCard);

        var fifteenScore = fifteens.Count * GameRules.Points.Fifteen;
        var flushScore = flush?.Count == 5 ? GameRules.Points.Flush + 1 :
            flush?.Count == 4 && !isCrib ? GameRules.Points.Flush : 0;
        var pairScore = pairs.Count * GameRules.Points.Pair;
        var runScore = runs.Sum(c => c.Count);
        var hisNobsScore = hisNobs == null ? 0 : GameRules.Points.Nobs;

        var totalScore = fifteenScore + flushScore + pairScore + runScore + hisNobsScore;

        var points = new CalculatedPoints(totalScore, fifteenScore, pairScore, runScore, flushScore, hisNobsScore);
        var combinations = new CalculatedCombinations(fifteens, pairs, runs, flush, hisNobs);

        return new CalculatedResult(points, combinations);
    }

    /// <summary>
    /// Count the points in the pile.
    /// </summary>
    /// <param name="pile">The current pile.</param>
    /// <returns>The points in the pile.</returns>
    public int CountPlayPoints(IList<Card> pile)
    {
        if (pile.Count < 2)
        {
            return 0;
        }

        var scored = 0;

        // count 15s
        scored += IsFifteen(pile) ? GameRules.Points.Fifteen : 0;

        // count pairs
        if (pile.Count > 3 && pile.TakeLast(4).GroupBy(c => c.Rank).Count() == 1)
        {
            scored += GameRules.Points.DoublePairRoyal;
        }
        else if (pile.Count > 2 && pile.TakeLast(3).GroupBy(c => c.Rank).Count() == 1)
        {
            scored += GameRules.Points.PairRoyal;
        }
        else
        {
            scored += IsSameKind(pile.TakeLast(2)) ? GameRules.Points.Pair : 0;
        }

        // count runs
        var count = pile.Count;
        while (count > 2)
        {
            if (IsRun(pile.TakeLast(count).ToList()))
            {
                scored += count;
                break;
            }

            count--;
        }

        // 31 count
        if (SumValues(pile) == GameRules.Points.MaxPlayCount)
        {
            scored += 2;
        }

        return scored;
    }


    /// <summary>
    /// Returns collection of all unique sets of cards that add up to 15.
    /// </summary>
    /// <param name="combinationsToCount">Lookup of all card combinations by permutation count.</param>
    /// <returns>List of all 15 combinations.</returns>
    public List<IReadOnlyList<Card>> FindFifteens(Dictionary<int, List<IReadOnlyList<Card>>> combinationsToCount) =>
        combinationsToCount
            .Where(d => d.Key > 1)
            .SelectMany(kv => kv.Value)
            .Where(IsFifteen)
            .ToList();

    /// <summary>
    /// All four cards in the hand are of the same suit.
    /// </summary>
    /// <param name="playersHand">The players hand.</param>
    /// <param name="starterCard">The starter card.</param>
    /// <returns>List of cards that make a flush or null if no flush found.</returns>
    public List<Card>? FindFlush(List<Card> playersHand, Card starterCard)
    {
        if (playersHand.Count < 4)
        {
            return new List<Card>(0);
        }

        var fourCardFlush = playersHand.GroupBy(c => c.Suit).Where(g => g.Count() == 4).ToList();

        if (fourCardFlush.Count == 1)
        {
            // check for 5 card flush
            if (fourCardFlush.First().Key == starterCard.Suit)
            {
                var returnHand = playersHand.ToList();
                returnHand.Add(starterCard);
                return returnHand;
            }

            return playersHand.ToList();
        }

        return null;
    }

    /// <summary>
    /// A pair of cards of a kind.
    /// </summary>
    /// <param name="combinations">Lookup of all card combinations by permutation count.</param>
    /// <returns>Returns all pairs found.</returns>
    public List<IReadOnlyList<Card>> FindPairs(Dictionary<int, List<IReadOnlyList<Card>>> combinations) =>
        combinations[2].Where(c => c[0].Rank == c[1].Rank).ToList();

    /// <summary>
    /// Find all unique runs of 3, 4, or 5 cards.
    /// </summary>
    /// <param name="combinations">Lookup of all card combinations by permutation count.</param>
    /// <returns>Set of all combinations of runs.</returns>
    // only looking for runs of 3, 4, and 5
    public List<IReadOnlyList<Card>> FindRuns(Dictionary<int, List<IReadOnlyList<Card>>> combinations)
    {
        var returnList = combinations[5].Where(IsRun).ToList();

        if (returnList.Count > 0)
        {
            return returnList;
        }

        returnList.AddRange(combinations[4].Where(IsRun));

        if (returnList.Count > 0)
        {
            return returnList;
        }

        returnList.AddRange(combinations[3].Where(IsRun));

        return returnList;
    }

    /// <summary>
    /// When the Jack of the same suit matches the starter card.
    /// </summary>
    /// <param name="cards">Player's hand.</param>
    /// <param name="starterCard">The starter or the cut.</param>
    /// <returns>The Nob.</returns>
    public Card? FindNobs(IEnumerable<Card> cards, Card starterCard)
    {
        return cards.SingleOrDefault(c => c.Rank == Rank.Jack && c.Suit == starterCard.Suit);
    }

    /// <summary>
    ///  Separate combination of two or more cards totaling exactly fifteen.
    /// </summary>
    /// <param name="set">Set of cards to check.</param>
    /// <returns>True of set adds up to 15.</returns>
    public bool IsFifteen(IEnumerable<Card> set)
    {
        return SumValues(set) == 15;
    }

    /// <summary>
    /// Sum of the value of the cards.
    /// </summary>
    /// <param name="set">Set of cards to sum.</param>
    /// <returns>The sum of the values.</returns>
    public int SumValues(IEnumerable<Card> set)
    {
        return set.Sum(c => ValueStrategy.GetValue(c));
    }

    /// <summary>
    /// Check if cards are all of same kind..
    /// </summary>
    /// <param name="set">Set of cards to check.</param>
    /// <returns>True of all cards are of the same kind.</returns>
    public bool IsSameKind(IEnumerable<Card> set)
    {
        var x = set.DistinctBy(c => c.Rank);
        return x.Count() == 1;
    }

    /// <summary>
    /// Three or more consecutive cards (regardless of suit).
    /// </summary>
    /// <param name="set">The set of cards.</param>
    /// <returns>True if the cards form a run.</returns>
    public bool IsRun(IEnumerable<Card> set)
    {
        var setList = set.ToList();
        if (setList.Count < 3) return false;
        var cardinalSet = setList.Select(c => (int)c.Rank);
        return IsContinuous(cardinalSet);
    }

    /// <summary>
    /// Check if the value are continuous.
    /// </summary>
    /// <param name="values">The set of values.</param>
    /// <returns>True if all the values in set are continuous.</returns>
    public bool IsContinuous(IEnumerable<int> values)
    {
        var orderedSet = values.OrderBy(c => c).ToList();

        for (int i = 0; i < orderedSet.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            if (orderedSet[i - 1] != orderedSet[i] - 1)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Dictionary of the combinations.  The key is "k" in k-combination combinatorial mathematics. Zero is not calculated.
    /// The Value is the set of the combination sets.
    /// </summary>
    /// <typeparam name="T">The type of class to find combinations.</typeparam>
    /// <param name="source">The set of cards.</param>
    /// <returns>A dictionary where the key is the number of combinations.</returns>
    public Dictionary<int, List<IReadOnlyList<T>>> GetCombinations<T>(IList<T> source) =>
        Enumerable.Range(1, source.Count)
            .ToDictionary(size => size, size => new Combinations<T>(source, size).ToList());
}