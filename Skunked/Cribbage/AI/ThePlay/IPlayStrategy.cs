using System.Collections.Generic;
using Cribbage.Rules;
using Skunked;

namespace Cribbage.AI.ThePlay
{
    public interface IPlayStrategy
    {
        Card DetermineCardToThrow(CribGameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft);
    }
}