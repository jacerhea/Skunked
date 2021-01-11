using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Rules;

namespace Skunked.Players
{
    public interface IGameRunnerPlayer
    {
        int Id { get; }

        /// <summary>
        /// Deal Hand and return cards that will go back in crib
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        List<Card> DetermineCardsToThrow(IEnumerable<Card> hand);
        Card DetermineCardsToPlay(GameRules gameRules, List<Card> pile, List<Card> handLeft);
        Card CutCards(List<Card> cardsToChoose);
        int CountHand(Card card, IEnumerable<Card> hand);
    }
}