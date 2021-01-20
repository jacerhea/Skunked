﻿using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    ///
    /// </summary>
    public class HandCountedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandCountedEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="playerId">The id of the player.</param>
        /// <param name="countedScore"></param>
        public HandCountedEvent(Guid gameId, int version, int playerId, int countedScore)
            : base(gameId, version)
        {
            PlayerId = playerId;
            CountedScore = countedScore;
        }

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public int PlayerId { get; }

        public int CountedScore { get; }
    }
}