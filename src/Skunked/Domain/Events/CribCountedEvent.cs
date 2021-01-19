﻿using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// The crib has been scored.
    /// </summary>
    public class CribCountedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CribCountedEvent"/> class.
        /// </summary>
        /// <param name="gameId">The Id of the game.</param>
        /// <param name="version">The event number.</param>
        /// <param name="playerId">The Id of the player.</param>
        /// <param name="countedScore">The counted score.</param>
        public CribCountedEvent(Guid gameId, int version, int playerId, int countedScore)
            : base(gameId, version)
        {
            PlayerId = playerId;
            CountedScore = countedScore;
        }

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the score the player counted for the crib.
        /// </summary>
        public int CountedScore { get; }
    }
}