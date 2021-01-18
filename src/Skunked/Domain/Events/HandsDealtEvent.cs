using System;
using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.Domain.Events
{
    public class HandsDealtEvent : StreamEvent
    {
        public HandsDealtEvent(Guid gameId, int version, List<PlayerHand> hands) : base(gameId, version)
        {
            Hands = hands;
        }

        public List<PlayerHand> Hands { get; }
    }
}
