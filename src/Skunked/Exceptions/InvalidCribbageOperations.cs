namespace Skunked.Exceptions
{
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
        ///
        /// </summary>
        CardsHaveBeenThrown,

        /// <summary>
        ///
        /// </summary>
        InvalidPlayer,

        /// <summary>
        ///
        /// </summary>
        InvalidStateForPlay,

        /// <summary>
        ///
        /// </summary>
        InvalidCard,

        /// <summary>
        ///
        /// </summary>
        InvalidStateForCount,

        /// <summary>
        ///
        /// </summary>
        InvalidStateForCribCount,

        /// <summary>
        ///
        /// </summary>
        InvalidShowCount,

        /// <summary>
        ///
        /// </summary>
        PlayerHasAlreadyCounted,

        /// <summary>
        ///
        /// </summary>
        GameFinished,

        /// <summary>
        ///
        /// </summary>
        InvalidRequest,
    }
}
