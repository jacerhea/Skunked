using System;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    ///
    /// </summary>
    public class StarterCardSelectedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StarterCardSelectedEvent"/> class.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="version"></param>
        /// <param name="starter"></param>
        public StarterCardSelectedEvent(Guid gameId, int version, Card starter)
            : base(gameId, version)
        {
            Starter = starter;
        }

        /// <summary>
        ///
        /// </summary>
        public Card Starter { get; }
    }
}
