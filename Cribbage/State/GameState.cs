using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.State
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public List<PlayerScore> PlayerScores { get; set; }
        public GameRules GameRules { get; set; }
        public OpeningRoundState OpeningRoundState { get; set; }

        public List<RoundState> Rounds { get; set; }

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
