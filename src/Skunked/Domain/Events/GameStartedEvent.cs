using System;
using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.Domain.Events
{
    public class GameStartedEvent : StreamEvent
    {
        public GameStartedEvent(Guid gameId, int version, List<int> players, GameRules rules)
            : base(gameId, version)
        {
            Players = players;
            Rules = rules;
        }

        public List<int> Players { get; }

        public GameRules Rules { get; }
    }
}
