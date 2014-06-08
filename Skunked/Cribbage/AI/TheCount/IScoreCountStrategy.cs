using System.Collections.Generic;
using Skunked;

namespace Cribbage.AI.TheCount
{
    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }
}