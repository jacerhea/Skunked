using System;

namespace Skunked.Domain.Events
{
    public class CribCountedEvent : StreamEvent
    {

        public CribCountedEvent(Guid gameId, int version, int playerId, int countedScore) : base(gameId, version)
        {
            PlayerId = playerId;
            CountedScore = countedScore;
        }

        /// <summary>
        /// The player id. 
        /// </summary>
        public int PlayerId { get; }
        public int CountedScore { get; }
    }
}