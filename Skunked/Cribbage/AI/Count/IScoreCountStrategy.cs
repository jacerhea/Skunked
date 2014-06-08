using System.Collections.Generic;

namespace Skunked.AI.Count
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}