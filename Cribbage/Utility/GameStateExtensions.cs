using System.Linq;
using Skunked.State;

namespace Skunked.Utility
{
    public static class GameStateExtensions
    {
        public static bool IsGameFinished(this GameState game)
        {
            return game.Scores.Any(ps => ps.Score >= game.Rules.WinningScore);
        }

        public static RoundState GetCurrentRound(this GameState game)
        {
            return game.Rounds.MaxBy(round => round.Round);
        }
    }
}
