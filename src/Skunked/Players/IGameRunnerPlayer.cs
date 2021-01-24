using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Rules;

namespace Skunked.Players
{
    /// <summary>
    /// An interface to implement an automated player's decisions.
    /// </summary>
    public interface IGameRunnerPlayer
    {
        /// <summary>
        /// Gets the id of the player.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Deal Hand and return cards that will go back in crib.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        List<Card> DetermineCardsToThrow(IEnumerable<Card> hand);

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameRules"></param>
        /// <param name="pile"></param>
        /// <param name="handLeft"></param>
        /// <returns></returns>
        Card DetermineCardsToPlay(GameRules gameRules, List<Card> pile, List<Card> handLeft);

        /// <summary>
        ///
        /// </summary>
        /// <param name="cardsToChoose"></param>
        /// <returns></returns>
        Card CutCards(List<Card> cardsToChoose);

        /// <summary>
        /// Count the score with the given hand and starter starter.
        /// </summary>
        /// <param name="starter"></param>
        /// <param name="hand"></param>
        /// <returns></returns>
        int CountHand(Card starter, IEnumerable<Card> hand);
    }
}