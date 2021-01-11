using System.Linq;
using Skunked.Domain.State;

namespace Skunked.Utility
{
    public static class GameStateExtensions
    {
        public static bool IsGameFinished(this GameState game)
        {
            return game.TeamScores.Any(teamScore => teamScore.Score >= game.GameRules.WinningScore);
        }

        public static RoundState GetCurrentRound(this GameState game)
        {
            return game.Rounds.MaxBy(round => round.Round);
        }

        public static int GetNextPlayerFrom(this GameState gameState, int playerId)
        {
            var currentPlayer = gameState.PlayerIds.Single(id => id == playerId);
            return gameState.PlayerIds.NextOf(currentPlayer);
        }
    }
}
