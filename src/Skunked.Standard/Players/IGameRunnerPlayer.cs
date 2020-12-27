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
        List<Card> DealHand(IList<Card> hand);
        Card PlayShow(GameRules gameRules, List<Card> pile, List<Card> handLeft);
        Card ChooseCard(List<Card> cardsToChoose);
        int CountHand(Card card, IEnumerable<Card> hand);
    }

    public interface IScoreCountStrategy
    {
        int GetCount(Card card, IEnumerable<Card> hand);
    }

    public interface IPlayStrategy
    {
        Card DetermineCardToThrow(GameRules gameRules, IList<Card> pile, IEnumerable<Card> handLeft);
    }

    public interface IDecisionStrategy
    {
        /// <summary>
        /// Take a dealt cribbage hand and return which cards should be thrown.
        /// </summary>
        /// <param name="hand">dealt hand</param>
        /// <returns>cards to throw</returns>
        IEnumerable<Card> DetermineCardsToThrow(IEnumerable<Card> hand);
    }
}