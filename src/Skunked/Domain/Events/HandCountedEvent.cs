using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    ///
    /// </summary>
    public class HandCountedEvent : StreamEvent
    {
        public HandCountedEvent(Guid gameId, int version, int playerId, int countedScore) : base(gameId, version)
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