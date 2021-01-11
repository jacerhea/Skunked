using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.Domain.Events
{
    public class GameStartedEvent : StreamEvent
    {
        public List<int> Players { get; set; } = new();
        public GameRules Rules { get; set; }
    }
}
