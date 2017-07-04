using System.Collections.Generic;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.AI.Show
{
    public class PercentageScoreCountStrategy : IScoreCountStrategy
    {
        private readonly int _percentageCorrect;
        private readonly ScoreCalculator _scoreCalculator;

        public PercentageScoreCountStrategy(int percentageCorrect = 100, ScoreCalculator scoreCalculator = null)
        {
            _percentageCorrect = percentageCorrect;
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        public int GetCount(Card card, IEnumerable<Card> hand)
        {
            var randomPercentage = RandomProvider.GetThreadRandom().Next(0, 100);
            if(randomPercentage > _percentageCorrect)
            {
                //todo: come up with better guess
                return 10;
            }
            return _scoreCalculator.CountShowScore(card, hand).Score;
        }
    }
}
