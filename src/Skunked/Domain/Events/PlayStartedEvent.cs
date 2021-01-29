using System;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Event when the play has started.
    /// </summary>
    public class PlayStartedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayStartedEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="round"></param>
        public PlayStartedEvent(Guid gameId, int version, int round)
            : base(gameId, version)
        {
            Round = round;
        }

        public int Round { get; }
    }
}
