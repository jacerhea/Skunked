using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Base event
    /// </summary>
    public abstract class StreamEvent
    {
        /// <summary>
        /// Identifier of the game.
        /// </summary>
        public Guid GameId { get; set; }

        /// <summary>
        /// Version of the game state.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The time stamp this event took palce.
        /// </summary>
        public DateTimeOffset Occurred { get; set; } = DateTimeOffset.Now;
    }
}
