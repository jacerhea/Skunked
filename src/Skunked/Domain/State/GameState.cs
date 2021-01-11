using System;
using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.Domain.State
{
    public class GameState
    {
        public Guid Id { get; set; }
        public List<int> PlayerIds { get; set; }
        public List<PlayerScore> IndividualScores { get; set; } = new();
        public List<TeamScore> TeamScores { get; set; } = new();
        public GameRules GameRules { get; set; }

        public OpeningRound OpeningRound { get; set; }
        public List<RoundState> Rounds { get; set; } = new();

        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
