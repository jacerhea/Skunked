using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Order.Interface;
using Cribbage.Rules;
using Skunked;

namespace Cribbage.AI.ThePlay
{
    public class LowestCardPlayStrategy : BasePlay, IPlayStrategy
    {
        private readonly IOrderStrategy _orderStrategy;

        public LowestCardPlayStrategy(IOrderStrategy orderStrategy)
        {
            if (orderStrategy == null) throw new ArgumentNullException("orderStrategy");
            _orderStrategy = orderStrategy;
        }

        public Card DetermineCardToThrow(CribGameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft)
        {
            ArgumentCheck(pile, handLeft);
            return handLeft.OrderBy(c => _orderStrategy.Order(c)).ThenBy(c => (int) c.Suit).First();
        }
    }
}