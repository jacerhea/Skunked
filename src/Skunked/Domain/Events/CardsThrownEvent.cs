using System;
using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Event when a cards have been thrown to the crib.
    /// </summary>
    public class CardsThrownEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardsThrownEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="playerId">The id of the player.</param>
        /// <param name="thrown">The cards thrown to the crib.</param>
        public CardsThrownEvent(Guid gameId, int version, int playerId, List<Card> thrown)
            : base(gameId, version)
        {
            PlayerId = playerId;
            Thrown = thrown;
        }

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the cards throw to the crib.
        /// </summary>
        public List<Card> Thrown { get; }
    }
}