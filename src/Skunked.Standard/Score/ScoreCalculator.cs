using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.Cards;
using Skunked.Cards.Order;
using Skunked.Cards.Value;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Score
{
    public class ScoreCalculator
    {
        private readonly ICardValueStrategy _valueStrategy;


        public ScoreCalculator(ICardValueStrategy valueStrategy = null)
        {
            _valueStrategy = valueStrategy ?? new AceLowFaceTenCardValueStrategy();
        }

        /// <summary>
        /// Check cut card for dealer
        /// </summary>
        /// <param name="cut"></param>
        /// <returns></returns>
        public int CountCut(Card cut) => cut.Rank == Rank.Jack ? GameRules.Scores.NibsScore : 0;

        public ScoreCalculatorResult CountShowScore(Card starterCard, IEnumerable<Card> playerHand)
        {
            var playerHandList = playerHand.ToList();
            var completeSet = playerHandList.Append(starterCard).ToList();
            var allCombinations = GetCombinations(completeSet);

            var fifteens = CountFifteens(allCombinations);
            var flush = CountFlush(playerHandList, starterCard);
            var pairs = CountPairs(allCombinations);
            var runs = CountRuns(allCombinations);
            var hisNobs = Nobs(playerHandList, starterCard);

            var fifteenScore = fifteens.Count * GameRules.Scores.FifteenScore;
            var flushScore = flush.Count == 4 ? GameRules.Scores.FourCardFlush : flush.Count == 5 ? GameRules.Scores.FiveCardFlush : 0;
            var pairScore = pairs.Count * GameRules.Scores.PairScore;
            var runScore = runs.Sum(c => c.Count);
            var hisNobsScore = hisNobs.Count == 0 ? 0 : GameRules.Scores.NobsScore;

            var totalScore = fifteenScore + flushScore + pairScore + runScore + hisNobsScore;

            var scoreResult = new ScoreCalculatorResult(fifteens, pairs, runs, flush, hisNobs, totalScore, fifteenScore, pairScore, runScore, flushScore, hisNobsScore);

            return scoreResult;
        }

        public int CountThePlay(IList<Card> pile)
        {
            if (pile.Count < 2)
            {
                return 0;
            }

            var scored = 0;

            //count 15s
            scored += IsFifteen(pile) ? GameRules.Scores.FifteenScore : 0;

            //count pairs
            if (pile.Count > 3 && pile.TakeLast(4).GroupBy(c => c.Rank).Count() == 1)
            {
                scored += GameRules.Scores.DoublePairRoyalScore;
            }
            else if (pile.Count > 2 && pile.TakeLast(3).GroupBy(c => c.Rank).Count() == 1)
            {
                scored += GameRules.Scores.PairRoyalScore;
            }
            else
            {
                scored += AreSameKind(pile.TakeLast(2)) ? GameRules.Scores.PairScore : 0;
            }

            //count runs
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

            //31 count
            if (SumValues(pile) == GameRules.Scores.PlayMaxScore)
            {
                scored += 2;
            }

            return scored;
        }


        public List<IList<Card>> CountFifteens(Dictionary<int, List<IList<Card>>> combinationsToCount)
        {
            return combinationsToCount
                .Where(d => d.Key > 1)
                .SelectMany(kv => kv.Value)
                .Where(IsFifteen)
                .ToList();
        }

        /// <summary>
        /// Returns the set of either all four cards in the hand if they are all of the same suit,
        /// or all five cards if the starter card matches the other four cards suit.
        /// </summary>
        /// <param name="playersHand"></param>
        /// <param name="starterCard"></param>
        /// <returns></returns>
        public List<Card> CountFlush(List<Card> playersHand, Card starterCard)
        {
            if (playersHand == null) throw new ArgumentNullException(nameof(playersHand));
            if (starterCard == null) throw new ArgumentNullException(nameof(starterCard));
            if (playersHand.Count < 4) { return new List<Card>(0); }

            var fourCardFlush = playersHand.GroupBy(c => c.Suit).Where(g => g.Count() > 3).ToList();

            if (fourCardFlush.Count == 1)
            {
                //check for 5 card flush
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
        /// A pair of cards of a kind
        /// </summary>
        /// <param name="combinationsToCheck"></param>
        /// <returns>Returns all pairs found.</returns>
        public List<IList<Card>> CountPairs(Dictionary<int, List<IList<Card>>> combinationsToCheck)
        {
            var combinationsOfTwoCards = combinationsToCheck[2];
            //not using "AreSameKind" method to improve performance.
            return combinationsOfTwoCards.Where(c => c[0].Rank == c[1].Rank).ToList();
        }

        /// <summary>
        /// Three consecutive cards (regardless of suit)
        /// </summary>
        /// <param name="combinationsToCount"></param>
        /// <returns></returns>
        //only looking for runs of 3,4, and 5
        public List<IList<Card>> CountRuns(Dictionary<int, List<IList<Card>>> combinationsToCount)
        {
            if (combinationsToCount == null) throw new ArgumentNullException(nameof(combinationsToCount));
            var returnList = combinationsToCount[5].Where(IsRun).ToList();

            if (returnList.Count > 0)
            {
                return returnList;
            }

            returnList.AddRange(combinationsToCount[4].Where(IsRun));

            if (returnList.Count > 0)
            {
                return returnList;
            }

            returnList.AddRange(combinationsToCount[3].Where(IsRun));

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
            return cards.Sum(c => _valueStrategy.GetValue(c));
        }

        public int GoValue => GameRules.Scores.Go;

        /// <summary>
        ///  Separate combination of two or more cards totaling exactly fifteen
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public bool IsFifteen(IList<Card> cards)
        {
            return SumValues(cards) == 15;
        }

        public bool AreSameKind(IEnumerable<Card> cards)
        {
            return cards.DistinctBy(c => c.Rank).Count() == 1;
        }

        /// <summary>
        /// Three or more consecutive cards (regardless of suit)
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        public bool IsRun(IList<Card> combo)
        {
            if (combo == null) throw new ArgumentNullException(nameof(combo));
            if (combo.Count < 3) return false;
            var cardinalSet = combo.Select(c => (int)c.Rank);
            return AreContinuous(cardinalSet);
        }

        public bool AreContinuous(IEnumerable<int> values)
        {
            var orderedSet = values.OrderBy(c => c).ToList();
            return Enumerable.Range(1, orderedSet.Count - 1).All(value => orderedSet[value] - 1 == orderedSet[value - 1]);
        }

        /// <summary>
        /// Dictionary of the combinations.  The key is "k" in k-combination combinatorial mathematics. Zero is not calculated.
        /// The Value is the set of the combination sets
        /// </summary>
        /// <param name="sourceSet"></param>
        /// <returns></returns>
        public Dictionary<int, List<IList<Card>>> GetCombinations(IList<Card> sourceSet)
        {
            var returnLookup = new Dictionary<int, List<IList<Card>>>(sourceSet.Count);

            foreach (var value in Enumerable.Range(1, sourceSet.Count))
            {
                returnLookup.Add(value, new Combinations<Card>(sourceSet, value).ToList());
                //var comboGen = ;
                //var  x = comboGen.sel
                //foreach (var set in comboGen)
                //{
                //    returnLookup[value].Add(set);
                //}
            }

            return returnLookup;
        }
    }
}