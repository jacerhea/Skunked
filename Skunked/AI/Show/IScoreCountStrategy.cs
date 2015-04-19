using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.AI.Show
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}