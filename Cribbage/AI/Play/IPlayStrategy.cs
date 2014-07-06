using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.AI.Play
{
    public interface IPlayStrategy
    {
        Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft);
    }
}