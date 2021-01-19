using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Base event.
    /// </summary>
    public abstract class StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEvent"/> class.
        /// </summary>
        /// <param name="gameId">Identifier of the game.</param>
        /// <param name="version">Version of the game state.</param>
        protected StreamEvent(Guid gameId, int version)
        {
            GameId = gameId;
            Version = version;
        }

        /// <summary>
        /// Gets identifier of the game.
        /// </summary>
        public Guid GameId { get; }

        /// <summary>
        /// Gets version of the game state.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets the time stamp this event took place.
        /// </summary>
        public DateTimeOffset Occurred { get; } = DateTimeOffset.Now;
    }
}
