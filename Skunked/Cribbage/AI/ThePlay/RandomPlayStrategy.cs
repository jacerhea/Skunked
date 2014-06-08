﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.PlayingCards;
using Cribbage.Rules;
using Cribbage.Score.Interface;
using Skunked;

namespace Cribbage.AI.ThePlay
{
    public class RandomPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;
        private Random _random;

        public RandomPlayStrategy(IScoreCalculator scoreCalculator, ICardValueStrategy valueStrategy)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (valueStrategy == null) throw new ArgumentNullException("valueStrategy");
            _scoreCalculator = scoreCalculator;
            _valueStrategy = valueStrategy;
            _random = new Random();
        }

        public Card DetermineCardToThrow(CribGameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);

            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= gameRules.PlayMaxScore).ToList();

            var randomIndex = validPlays.Count() - 1;
            return validPlays.ElementAt(randomIndex);
        }
    }
}