using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards.Value;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Score.Interface;
using Skunked.Utility;

namespace Skunked.AI.Play
{
    public class MaxPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;

        public MaxPlayStrategy(IScoreCalculator scoreCalculator = null, ICardValueStrategy valueStrategy = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _valueStrategy = valueStrategy ?? new AceLowFaceTenCardValueStrategy();
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            handLeft = handLeft.ToList();

            if(pile.Count == 0)
            {
                return StandardFirstCardPlay(handLeft);
            }

            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= gameRules.PlayMaxScore).ToList();
            var maxScore = validPlays
                .Select(card => new CardScore(card, _scoreCalculator.CountThePlay(new List<Card>(pile) { card })))
                .MaxBy(cs => cs.Score);


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