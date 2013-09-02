using System.Collections.Generic;

namespace Cribbage.AI.TheCount
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}