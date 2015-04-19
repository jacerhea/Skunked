﻿using System.Collections.Generic;
using System.Linq;

namespace Skunked.PlayingCards
{
    public static class OrderStrategy
    {
        public static IEnumerable<T> OrderCards<T>(this IEnumerable<T> cards) where T : Card
        {
            var orderStrategy = new StandardOrder();
            return cards.OrderBy(orderStrategy.Order).ThenBy(c => c.Suit);
        }
    }
}
