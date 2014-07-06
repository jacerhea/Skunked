using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skunked.PlayingCards;
using Skunked.Utility;

namespace Skunked.Test
{
    [TestClass]
    public class CardTestFixture
    {
        [TestMethod]
        public void TestCardProperties()
        {
            IEnumerable<Rank> allRanks = EnumHelper.GetValues<Rank>();
            IEnumerable<Suit> allSuits = EnumHelper.GetValues<Suit>();

            foreach (var rank in allRanks)
            {
                foreach (var suit in allSuits)
                {
                    var card = new Card(rank, suit);
                    Assert.AreEqual(rank, card.Rank);
                    Assert.AreEqual(suit, card.Suit);
                }
            }
        }

        [TestMethod]
        public void TestCardEqualsTyped()
        {
            IEnumerable<Rank> allRanks = EnumHelper.GetValues<Rank>();
            IEnumerable<Suit> allSuits = EnumHelper.GetValues<Suit>();

            foreach (var rank in allRanks)
            {
                foreach (var suit in allSuits)
                {
                    var card = new Card(rank, suit);
                    var clonedCard = new Card(rank, suit);

                    Assert.IsTrue(card.Equals(clonedCard));
                }
            }
        }
    }
}
