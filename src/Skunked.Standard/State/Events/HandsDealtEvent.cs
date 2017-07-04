using System.Collections.Generic;
using Skunked.Players;

namespace Skunked.State.Events
{
    public class HandsDealtEvent : StreamEvent
    {
        public List<PlayerIdHand> Hands { get; set; }
    }
}
