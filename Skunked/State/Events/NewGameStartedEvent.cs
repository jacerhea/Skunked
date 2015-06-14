using System.Collections.Generic;
using Skunked.Players;
using Skunked.Rules;

namespace Skunked.State.Events
{
    public class NewGameStartedEvent : Event
    {
        public List<int> Players { get; set; }
        public GameRules Rules { get; set; }
    }
}
