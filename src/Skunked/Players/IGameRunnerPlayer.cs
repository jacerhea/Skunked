using Skunked.Cards;
using Skunked.Rules;

namespace Skunked.Players;

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
    /// <param name="hand">The set of cards dealt to the player.</param>
    /// <returns>Set of Cards to throw in crib.</returns>
    List<Card> DetermineCardsToThrow(IEnumerable<Card> hand);

    /// <summary>
    /// Determine card to throw to play pile.
    /// </summary>
    /// <param name="gameRules">The rules of the game.</param>
    /// <param name="pile">Cards currently in the pile.</param>
    /// <param name="handLeft">Cards not yet played from hand.</param>
    /// <returns>The card to throw to the pile.</returns>
    Card DetermineCardsToPlay(GameRules gameRules, List<Card> pile, List<Card> handLeft);

    /// <summary>
    /// Get the card to cut.
    /// </summary>
    /// <param name="cardsToChoose">Cards to choose from.</param>
    /// <returns>The cut card.</returns>
    Card CutCards(IEnumerable<Card> cardsToChoose);

    /// <summary>
    /// Count the score with the given hand and starter starter.
    /// </summary>
    /// <param name="starter">The starter card or cut.</param>
    /// <param name="hand">The player's hand.</param>
    /// <returns>The points counted.</returns>
    int CountHand(Card starter, IEnumerable<Card> hand);
}