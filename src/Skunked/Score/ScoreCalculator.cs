﻿using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.Cards;
using Skunked.Cards.Value;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Score
{
    /// <summary>
    /// Calculates points scored in shows and plays.
    /// </summary>
    public class ScoreCalculator
    {
        private static readonly AceLowFaceTenCardValueStrategy ValueStrategy = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreCalculator"/> class.
        /// </summary>
        public ScoreCalculator() { }

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

            var fifteens = CountFifteens(allCombinations);
            var flush = CountFlush(playerHandList, starterCard);
            var pairs = CountPairs(allCombinations);
            var runs = CountRuns(allCombinations);
            var hisNobs = Nobs(playerHandList, starterCard);

            var fifteenScore = fifteens.Count * GameRules.Points.Fifteen;
            var flushScore = flush.Count == 5 ? GameRules.Points.Flush + 1 :
                flush.Count == 4 && !isCrib ? GameRules.Points.Flush : 0;
            var pairScore = pairs.Count * GameRules.Points.Pair;
            var runScore = runs.Sum(c => c.Count);
            var hisNobsScore = hisNobs.Count == 0 ? 0 : GameRules.Points.Nobs;

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
                scored += AreSameKind(pile.TakeLast(2)) ? GameRules.Points.Pair : 0;
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
        public List<IList<Card>> CountFifteens(Dictionary<int, List<IList<Card>>> combinationsToCount) =>
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
        /// <returns>List of cards that make a flush.</returns>
        public List<Card> CountFlush(List<Card> playersHand, Card starterCard)
        {
            if (playersHand.Count < 4) { return new List<Card>(0); }

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

            return new List<Card>(0);
        }

        /// <summary>
        /// A pair of cards of a kind.
        /// </summary>
        /// <param name="combinations">Lookup of all card combinations by permutation count.</param>
        /// <returns>Returns all pairs found.</returns>
        public List<IList<Card>> CountPairs(Dictionary<int, List<IList<Card>>> combinations) =>
            combinations[2].Where(c => c[0].Rank == c[1].Rank).ToList();

        /// <summary>
        /// Three consecutive cards (regardless of suit).
        /// </summary>
        /// <param name="combinations">Lookup of all card combinations by permutation count.</param>
        /// <returns></returns>
        // only looking for runs of 3,4, and 5
        public List<IList<Card>> CountRuns(Dictionary<int, List<IList<Card>>> combinations)
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
        /// <param name="cards"></param>
        /// <param name="starterCard"></param>
        /// <returns></returns>
        public IList<Card> Nobs(IEnumerable<Card> cards, Card starterCard)
        {
            return cards.Where(c => c.Rank == Rank.Jack && c.Suit == starterCard.Suit).ToList();
        }

        public int SumValues(IEnumerable<Card> cards)
        {
            return cards.Sum(c => ValueStrategy.GetValue(c));
        }

        /// <summary>
        ///  Separate combination of two or more cards totaling exactly fifteen.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public bool IsFifteen(IList<Card> cards)
        {
            return SumValues(cards) == 15;
        }

        /// <summary>
        /// Three or more consecutive cards (regardless of suit).
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public bool AreSameKind(IEnumerable<Card> cards)
        {
            return cards.DistinctBy(c => c.Rank).Count() == 1;
        }

        /// <summary>
        /// Three or more consecutive cards (regardless of suit).
        /// </summary>
        /// <param name="set">The set of cards.</param>
        /// <returns>True if the cards form a run.</returns>
        public bool IsRun(IList<Card> set)
        {
            if (set.Count < 3) return false;
            var cardinalSet = set.Select(c => (int)c.Rank);
            return AreContinuous(cardinalSet);
        }

        /// <summary>
        /// Check if the value are continuous.
        /// </summary>
        /// <param name="values">The set of values.</param>
        /// <returns>True if all the values in set are continuous.</returns>
        public bool AreContinuous(IEnumerable<int> values)
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
        /// <param name="source">The set of cards.</param>
        /// <returns>A dictionary where the key is the number of </returns>
        public Dictionary<int, List<IList<T>>> GetCombinations<T>(IList<T> source)
        {
            return Enumerable.Range(1, source.Count)
                .ToDictionary(size => size, size => new Combinations<T>(source, size).ToList());
        }
    }
}