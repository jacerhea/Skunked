using System;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// A card has been cut.
    /// </summary>
    public class CardCutEvent : StreamEvent
    {
        public CardCutEvent(Guid gameId, int version, int playerId, Card cutCard) : base(gameId, version)
        {
            PlayerId = playerId;
            CutCard = cutCard;
        }

        /// <summary>
        /// The player's id who cut the card.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// The card that was cut.
        /// </summary>
        public Card CutCard { get; }
    }
}