using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.State;
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
        public CribOpeningRoundState OpeningRoundState { get; set; }

        public List<RoundState> Rounds { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime LastUpdated { get; set; }

        public RoundState GetCurrentRound()
        {
            return Rounds.MaxBy(round => round.Round);
        }

        public bool IsGameFinished()
        {
            return PlayerScores.Any(ps => ps.Score >= GameRules.WinningScore);
        }
    }
}
