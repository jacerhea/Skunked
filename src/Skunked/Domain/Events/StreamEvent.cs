using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Base event
    /// </summary>
    public abstract class StreamEvent
    {
        public StreamEvent(Guid gameId, int version)
        {
            GameId = gameId;
            Version = version;
        }

        /// <summary>
        /// Identifier of the game.
        /// </summary>
        public Guid GameId { get; }

        /// <summary>
        /// Version of the game state.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// The time stamp this event took place.
        /// </summary>
        public DateTimeOffset Occurred { get; } = DateTimeOffset.Now;
    }
}
