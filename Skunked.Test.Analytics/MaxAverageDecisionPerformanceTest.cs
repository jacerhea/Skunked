using System.Linq;
using Skunked.AI.CardToss;
using Skunked.PlayingCards;
using Xunit;

namespace Skunked.Test.Analytics
{
    public class MaxAverageDecisionPerformanceTest
    {
        [Fact]
        public void RunManyMaxAverageDecisions()
        {
            var decision = new MaxAverageDecision();
            var deck = new Deck();

            foreach (var index in Enumerable.Range(0, 1000))
            {
                deck.Shuffle();
                decision.DetermineCardsToThrow(deck.Take(6));
            }
        }
    }
}
