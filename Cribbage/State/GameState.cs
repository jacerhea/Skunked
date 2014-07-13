using System;
using System.Collections.Generic;
using Skunked.Players;
using Skunked.Rules;

namespace Skunked.State
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public List<PlayerScore> Scores { get; set; }
        public GameRules Rules { get; set; }
        public OpeningRoundState OpeningRound { get; set; }

        public List<RoundState> Rounds { get; set; }

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
