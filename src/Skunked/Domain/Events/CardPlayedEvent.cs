using System;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// A Card has been played.
    /// </summary>
    public class CardPlayedEvent : StreamEvent
    {
        public CardPlayedEvent(Guid gameId, int version, int playerId, Card played)
            : base(gameId, version)
        {
            PlayerId = playerId;
            Played = played;
        }

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the Card being played.
        /// </summary>
        public Card Played { get; }
    }
}