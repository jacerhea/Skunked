using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards.Value;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Score.Interface;

namespace Skunked.AI.ThePlay
{
    public class MinPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;

        public MinPlayStrategy(IScoreCalculator scoreCalculator, ICardValueStrategy valueStrategy)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (valueStrategy == null) throw new ArgumentNullException("valueStrategy");
            _scoreCalculator = scoreCalculator;
            _valueStrategy = valueStrategy;             
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= gameRules.PlayMaxScore);
            var cardScores = new List<CardScore>(handLeft.Count());

            foreach (var card in validPlays)
            {
                var theoreticalPlayHand = new List<Card>(pile) { card };
                int score = _scoreCalculator.CountThePlay(theoreticalPlayHand);
                cardScores.Add(new CardScore(card, score));
            }

            var lowScore = cardScores.Min(cs => cs.Score);
            return cardScores.First(c => lowScore == c.Score).Card;
        }
    }
}