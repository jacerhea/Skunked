using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Event when a round started.
    /// </summary>
    public class RoundStartedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundStartedEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        public RoundStartedEvent(Guid gameId, int version)
            : base(gameId, version)
        {
        }
    }
}
