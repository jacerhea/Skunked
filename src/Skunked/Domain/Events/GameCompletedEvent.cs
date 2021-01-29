using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Event when the game has completed.
    /// </summary>
    public class GameCompletedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCompletedEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        public GameCompletedEvent(Guid gameId, int version)
            : base(gameId, version)
        {
        }
    }
}
