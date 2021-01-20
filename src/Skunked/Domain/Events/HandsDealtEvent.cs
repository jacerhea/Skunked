using System;
using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.Domain.Events
{
    public class HandsDealtEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandsDealtEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="hands"></param>
        public HandsDealtEvent(Guid gameId, int version, List<PlayerHand> hands)
            : base(gameId, version)
        {
            Hands = hands;
        }

        public List<PlayerHand> Hands { get; }
    }
}
