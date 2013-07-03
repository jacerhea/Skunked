using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Score;
using Games.Domain.MainModule.Entities.PlayingCards;
using Games.Domain.MainModule.Entities.PlayingCards.Order;
using Games.Domain.MainModule.Entities.PlayingCards.Value;
using Games.Infrastructure.CrossCutting;
using Games.Infrastructure.CrossCutting.Combinatorics;
using MoreLinq;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Score
{
    public class ScoreCalculator : IScoreCalculator
    {
        private readonly ICardValueStrategy _valueStrategy;
        private readonly IOrderStrategy _order;

        public ScoreCalculator(ICardValueStrategy valueStrategy, IOrderStrategy order)
        {
            if (valueStrategy == null) throw new ArgumentNullException("valueStrategy");
            if (order == null) throw new ArgumentNullException("order");
            _valueStrategy = valueStrategy;
            _order = order;
        }

        //Check cut card for dealer
        public int CountCut(ICard cut)
        {
            if (cut.Rank == Rank.Jack)
            {
                return 2;
            }
            return 0;
        }

        public CribScoreCalculatorResult CountShowScore(ICard cutCard, IEnumerable<ICard> playerHand)
        {
            var completeSet = new List<ICard>(playerHand) { cutCard };
            var allCombinations = GetCombinations(completeSet);

            var fifteens = CountFifteens(allCombinations);
            var flush = CountFlush(playerHand, cutCard);
            var pairs = CountPairs(allCombinations);
            var runs = CountRuns(allCombinations);
            var hisNobs = Nobs(playerHand, cutCard);

            int fifteenScore = fifteens.Count * 2;
            int flushScore = flush.Count;
            int pairScore = pairs.Count * 2;
            int runScore = runs.Sum(c => c.Count);
            int hisNobsScore = hisNobs.Count == 0 ? 0 : 1;

            int totalScore = fifteenScore + flushScore + pairScore + runScore + hisNobsScore;

            var scoreResult = new CribScoreCalculatorResult(fifteens, pairs, runs, flush, hisNobs, totalScore, fifteenScore, pairScore, runScore, flushScore, hisNobsScore);

            return scoreResult;
        }

        public int CountThePlay(IList<ICard> pile)
        {
            if (pile.Count < 2)
            {
                return 0;
            }

            int scored = 0;

            //count 15s
            scored += IsFifteen(pile) ? 2 : 0;

            //count pairs
            if (pile.Count > 3 && pile.TakeLast(4).GroupBy(c => c.Rank).Count() == 1)
            {
                scored += 12;
            }
            else if (pile.Count > 2 && pile.TakeLast(3).GroupBy(c => c.Rank).Count() == 1)
            {
                scored += 6;
            }
            else
            {
                scored += AreSameKind(pile.TakeLast(2)) ? 2 : 0;
            }

            //count runs
            int count = pile.Count;
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
            if (SumValues(pile) == 31)
            {
                scored += 2;
            }

            return scored;
        }

        public List<IList<ICard>> CountFifteens(Dictionary<int, List<IList<ICard>>> combinationsToCount)
        {
            return combinationsToCount
                .Where(d => d.Key > 1)
                .SelectMany(kv => kv.Value)
                .Where(IsFifteen)
                .ToList();
        }

        public List<ICard> CountFlush(IEnumerable<ICard> playersHand, ICard cutCard)
        {
            if (playersHand == null) throw new ArgumentNullException("playersHand");
            if (cutCard == null) throw new ArgumentNullException("cutCard");
            if (playersHand.Count() < 4) { return new List<ICard>(); }

            var fourCardFlush = playersHand.GroupBy(c => c.Suit).Where(g => g.Count() > 3).ToList();

            if (fourCardFlush.Count() == 1)
            {
                //check for 5 card flush
                if (fourCardFlush.First().Key == cutCard.Suit)
                {
                    var returnHand = playersHand.ToList();
                    returnHand.Add(cutCard);
                    return returnHand;

                }
                return playersHand.ToList();
            }
            return new List<ICard>(0);
        }

        public List<IList<ICard>> CountPairs(Dictionary<int, List<IList<ICard>>> combinationsToCheck)
        {
            var combinationsOfTwoCards = combinationsToCheck[2];

            return combinationsOfTwoCards.Where(AreSameKind).ToList();
        }

        //only looking for runs of 3,4, and 5
        public List<IList<ICard>> CountRuns(Dictionary<int, List<IList<ICard>>> combinationsToCount)
        {
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

        public IList<ICard> Nobs(IEnumerable<ICard> cards, ICard starterCard)
        {
            return cards.Where(c => c.Rank == Rank.Jack && c.Suit == starterCard.Suit).ToList();
        }

        public int SumValues(IEnumerable<ICard> cards)
        {
            return cards.Sum(c => _valueStrategy.ValueOf(c));
        }

        public int GetGoValue()
        {
            return 1;
        }

        public bool IsFifteen(IList<ICard> cards)
        {
            return SumValues(cards) == 15;
        }

        public bool AreSameKind(IEnumerable<ICard> cards)
        {
            return cards.DistinctBy(c => c.Rank).Count() == 1;
        }

        public bool IsRun(IList<ICard> combo)
        {
            if (combo == null) throw new ArgumentNullException("combo");
            if (combo.Count < 3) return false;
            var cardinalSet = combo.Select(c => _order.Order(c)).OrderBy(cardinal => cardinal);
            return AreContinuous(cardinalSet);
        }

        public bool AreContinuous(IEnumerable<int> values)
        {
            var orderedSet = values.OrderBy(c => c).ToList();

            return Enumerable.Range(1, orderedSet.Count - 1).All(value => orderedSet[value] - 1 == orderedSet[value - 1]);
        }

        /// <summary>
        /// Dictionary of the combinations.  The key is "k" in k-combination combinatorial mathmatics. Zero is not calculated.
        /// The Value is the set of the combination sets
        /// </summary>
        /// <param name="sourceSet"></param>
        /// <returns></returns>
        public Dictionary<int, List<IList<ICard>>> GetCombinations(IList<ICard> sourceSet)
        {
            var returnLookup = new Dictionary<int, List<IList<ICard>>>(sourceSet.Count);

            foreach (int value in Enumerable.Range(1, sourceSet.Count))
            {
                returnLookup.Add(value, new List<IList<ICard>>());
                var comboGen = new Combinations<ICard>(sourceSet, value);
                foreach (var set in comboGen)
                {
                    returnLookup[value].Add(set);
                }
            }

            return returnLookup;
        }
    }
}