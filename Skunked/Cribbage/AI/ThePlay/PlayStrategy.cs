using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.PlayingCards;
using Cribbage.Rules;
using Cribbage.Score;
using Cribbage.Score.Interface;
using Cribbage.Utility;
using Skunked;

namespace Cribbage.AI.ThePlay
{
    public class PlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;

        public PlayStrategy(IScoreCalculator scoreCalculator, ICardValueStrategy valueStrategy)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (valueStrategy == null) throw new ArgumentNullException("valueStrategy");
            _scoreCalculator = scoreCalculator;
            _valueStrategy = valueStrategy;
        }

        public Card DetermineCardToThrow(CribGameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);

            if(pile.Count == 0)
            {
                return StandardFirstCardPlay(handLeft);
            }

            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= gameRules.PlayMaxScore);
            var cardScores = new List<CardScore>(handLeft.Count());

            foreach (var card in validPlays)
            {
                var theoreticalPlayHand = new List<Card>(pile) { card };
                int score = _scoreCalculator.CountThePlay(theoreticalPlayHand);
                cardScores.Add(new CardScore(card, score));
            }

            var maxScore = cardScores.MaxBy(cs => cs.Score);

            if(maxScore.Score == 0)
            {
                return validPlays.MaxBy(c => _valueStrategy.ValueOf(c));
            }
            else
            {
                return maxScore.Card;
            }
        }
    }
}