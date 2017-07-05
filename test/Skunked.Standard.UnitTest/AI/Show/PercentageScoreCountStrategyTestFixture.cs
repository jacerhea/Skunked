
using System.Collections.Generic;
using System.Linq;
using Skunked.AI.Show;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.AI.Show
{
    public class PercentageScoreCountStrategyTestFixture
    {
        [Fact]
        public void Test_GetCount()
        {
            var asdfjklasd = new PercentageScoreCountStrategy();
            foreach (var v in Enumerable.Range(0, 400))
            {
                asdfjklasd.GetCount(new Card(), new List<Card> { new Card(), new Card(), new Card(), new Card() });
            }
            Assert.True(true);
        }
    }
}
