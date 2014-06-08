using System.Collections.Generic;

namespace Skunked.AI.TheCount
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}