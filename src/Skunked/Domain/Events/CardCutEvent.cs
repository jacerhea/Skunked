using System;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// A card has been cut.
    /// </summary>
    public class CardCutEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardCutEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="playerId">The id of the player.</param>
        /// <param name="cutCard"></param>
        public CardCutEvent(Guid gameId, int version, int playerId, Card cutCard)
            : base(gameId, version)
        {
            PlayerId = playerId;
            CutCard = cutCard;
        }

        /// <summary>
        /// Gets the player's id who cut the card.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the card that was cut.
        /// </summary>
        public Card CutCard { get; }
    }
}