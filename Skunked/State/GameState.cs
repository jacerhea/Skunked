using System;
using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.State
{
    public class GameState
    {
        public Guid Id { get; set; }
        public List<int> PlayerIds { get; set; }
        public List<PlayerScore> IndividualScores { get; set; }
        public List<TeamScore> TeamScores { get; set; }
        public GameRules GameRules { get; set; }

        public OpeningRoundState OpeningRound { get; set; }
        public List<RoundState> Rounds { get; set; }

        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
