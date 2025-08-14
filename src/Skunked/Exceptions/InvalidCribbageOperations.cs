namespace Skunked;

/// <summary>
/// Set of exception types that can be thrown by the game when a rule is violated.
/// </summary>
public enum InvalidCribbageOperation
{
    /// <summary>
    /// Player already cut a card from the deck.
    /// </summary>
    CutCardPlayerAlreadyCut,

    /// <summary>
    /// A player already this card from the deck.
    /// </summary>
    CutCardCardAlreadyCut,

    /// <summary>
    /// Player attempted to play when not their turn.
    /// </summary>
    NotPlayersTurn,

    /// <summary>
    /// The played card has already been played.
    /// </summary>
    CardHasBeenPlayed,

    /// <summary>
    /// Player can not rethrow their cards.
    /// </summary>
    CardsHaveBeenThrown,

    /// <summary>
    /// It is not the given players' turn.
    /// </summary>
    InvalidPlayer,

    /// <summary>
    /// The game is not currently in the play.
    /// </summary>
    InvalidStateForPlay,

    /// <summary>
    /// Card can not be used.  This may be because the player was not dealt the card.
    /// </summary>
    InvalidCard,

    /// <summary>
    /// The hand can not be counted in the current state of the game.
    /// </summary>
    InvalidStateForCount,

    /// <summary>
    /// The crib can not be counted in the current state of the game.
    /// </summary>
    InvalidStateForCribCount,

    /// <summary>
    /// The player announced a score that was not possible with the given hand.
    /// </summary>
    InvalidShowCount,

    /// <summary>
    /// The player has already counted their hand in this round.
    /// </summary>
    PlayerHasAlreadyCounted,

    /// <summary>
    /// The parameters of the game were violated.
    /// </summary>
    InvalidRequest
}