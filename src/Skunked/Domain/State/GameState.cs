using System;
using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.Domain.State
{
    /// <summary>
    /// Snapshot of a game's state.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Unique identifier of the game.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Version of the game.  Each event creates a new version of the game.
        /// </summary>
        public int Version { get; set; }

        public List<int> PlayerIds { get; set; }

        public List<PlayerScore> IndividualScores { get; set; } = new ();

        public List<TeamScore> TeamScores { get; set; } = new ();

        /// <summary>
        /// The rules used during the game.
        /// </summary>
        public GameRules GameRules { get; set; }

        public OpeningRound OpeningRound { get; set; }

        public List<RoundState> Rounds { get; set; } = new ();

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset LastUpdated { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }
    }
}
