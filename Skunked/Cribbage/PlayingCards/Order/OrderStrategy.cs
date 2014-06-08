using System.Collections.Generic;
using System.Linq;

namespace Skunked.PlayingCards.Order
{
    public static class OrderStrategy
    {
        public static IEnumerable<T> OrderCards<T>(this IEnumerable<T> cards) where T : Card
        {
            var orderStrategy = new StandardOrder();
            return cards.OrderBy(c => orderStrategy.Order(c)).ThenBy(c => c.Suit);
        }
    }
}
