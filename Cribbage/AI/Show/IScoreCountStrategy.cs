using System.Collections.Generic;

namespace Skunked.AI.Show
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}