using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.Domain.Events
{
    public class HandsDealtEvent : StreamEvent
    {
        public List<PlayerHand> Hands { get; set; }
    }
}
