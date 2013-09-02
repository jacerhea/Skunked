using System;
using System.Collections.Generic;
using Cribbage.Score.Interface;
using Cribbage.Utility;

namespace Cribbage.AI.TheCount
{
    public class PercentageScoreCountStrategy : IScoreCountStrategy
    {
        private readonly int _percentageCorrect;
        private readonly IScoreCalculator _scoreCalculator;
        private readonly Random _random = new Random();

        public PercentageScoreCountStrategy(int percentageCorrect, IScoreCalculator scoreCalculator)
        {
            if (!percentageCorrect.IsBetween(0, 100)) { throw new ArgumentOutOfRangeException("percentageCorrect"); }
            _percentageCorrect = percentageCorrect;
            _scoreCalculator = scoreCalculator;
        }

        public int GetCount(Card card, IEnumerable<Card> hand)
        {
            var x = _random.Next(0, 100);
            if(x < _percentageCorrect)
            {
                return 10;
            }
            return _scoreCalculator.CountShowScore(card, hand).Score;
        }
    }
}
