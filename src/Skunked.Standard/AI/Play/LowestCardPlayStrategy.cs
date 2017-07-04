using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;
using Skunked.Rules;

namespace Skunked.AI.Play
{
    public class LowestCardPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IOrderStrategy _orderStrategy;

        public LowestCardPlayStrategy(IOrderStrategy orderStrategy = null)
        {
            _orderStrategy = orderStrategy ?? new StandardOrder();
        }

        public Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            return handLeft.OrderBy(c => _orderStrategy.Order(c)).ThenBy(c => (int) c.Suit).First();
        }
    }
}