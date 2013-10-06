using System.Collections.Generic;
using Cribbage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skunked.Test.PlayingCards.Order
{
    [TestClass]
    public class OrderTest
    {

        [TestMethod]
        private void Cards()
        {
            var cards = new List<Card>
            {
                new Card(Rank.King),
                new Card(Rank.Five),
                new Card(Rank.Ace),
                new Card(Rank.Nine),
                new Card(Rank.Jack),
            };
            foreach (var card in cards)
            {
                //StandardOrderTest(card)
            }
        }   

        public void StandardOrderTest(Rank rank, int expectedValue)
        {
            //var cards = Cards();

            //var orderStrategy = new StandardOrder();

            //var sortedByOrderStrategy = cards.OrderBy(orderStrategy.Order).ToList();
            //var card = new Card(rank);

            //Assert.AreEqual(expectedValue, sortedByOrderStrategy.IndexOf(card));
        }


    }
}