using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.State.Events
{
    public class HandsDealtEvent : StreamEvent
    {
        public List<PlayerHand> Hands { get; set; }
    }
}
