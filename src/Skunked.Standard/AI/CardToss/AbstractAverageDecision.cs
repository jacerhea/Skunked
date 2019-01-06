using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using Skunked.PlayingCards;
using Skunked.Score;

namespace Skunked.AI.CardToss
{
    public abstract class AbstractAverageDecision
    {
        private readonly ScoreCalculator _scoreCalculator;

        protected AbstractAverageDecision(ScoreCalculator scoreCalculator = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hand">Dealt hand. 5 or 6 cards.</param>
        /// <returns></returns>
        protected IEnumerable<ComboPossibleScores> BaseAverageDecision(IEnumerable<Card> hand)
        {
            if (hand == null) throw new ArgumentNullException(nameof(hand));
            var handSet = new HashSet<Card>(hand);
            var combinations = new Combinations<Card>(handSet.ToList(), 4);
            var deck = new Deck();
            // the deck minus the given hand.
            var possibleCardsCut = deck.Except(handSet).ToList();

            var comboPossibleScoreses = GetPossibleCombos(combinations, possibleCardsCut).ToList();
            return comboPossibleScoreses;
        }

        /// <summary>
        /// Set of each possible combination of the given hand, cross producted with the possible starter cards.
        /// </summary>
        /// <param name="handCombinations"></param>
        /// <param name="possibleStarterCards"></param>
        /// <returns></returns>
        private IEnumerable<ComboPossibleScores> GetPossibleCombos(Combinations<Card> handCombinations, IReadOnlyCollection<Card> possibleStarterCards)
        {
            return handCombinations.Select(combo =>
            {
                var possibleScores = possibleStarterCards
                    .Select(cutCard => new ScoreWithCut { Cut = cutCard, Score = _scoreCalculator.CountShowScore(cutCard, combo).Score })
                    .ToList();
                return new ComboPossibleScores(combo, possibleScores);
            });
        }
    }
}