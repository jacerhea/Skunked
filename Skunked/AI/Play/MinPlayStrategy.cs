using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;

namespace Skunked.AI.Play
{
    public class MinPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly ScoreCalculator _scoreCalculator;
        private readonly ICardValueStrategy _valueStrategy;

        public MinPlayStrategy(ScoreCalculator scoreCalculator = null, ICardValueStrategy valueStrategy = null)
        {
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _valueStrategy = valueStrategy ?? new AceLowFaceTenCardValueStrategy();
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            int currentPileCount = _scoreCalculator.SumValues(pile);
            var validPlays = handLeft.Where(c => currentPileCount + _valueStrategy.ValueOf(c) <= GameRules.PlayMaxScore);
            var x = validPlays.Select(card => new {card, theoreticalPlayHand = new List<Card>(pile) {card}})
                .Select(a => new {t = a, score = _scoreCalculator.CountThePlay(a.theoreticalPlayHand)})
                .Select(anon => new CardScore(anon.t.card, anon.score))
                .ToList();

            var lowScore = x.Min(cs => cs.Score);
            return x.First(c => lowScore == c.Score).Card;
        }
    }
}