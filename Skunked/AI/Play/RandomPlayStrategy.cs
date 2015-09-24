using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;

namespace Skunked.AI.Play
{
    public class RandomPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly ScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;

        public RandomPlayStrategy(ScoreCalculator scoreCalculator = null, ICardValueStrategy valueStrategy = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _valueStrategy = valueStrategy ?? new AceLowFaceTenCardValueStrategy();
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);

            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= GameRules.PlayMaxScore).ToList();

            var randomIndex = validPlays.Count - 1;
            return validPlays.ElementAt(randomIndex);
        }
    }
}