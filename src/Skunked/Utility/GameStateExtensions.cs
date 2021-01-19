using System.Linq;
using Skunked.Domain.State;

namespace Skunked.Utility
{
    /// <summary>
    /// Set of convenience methods to aggregate state from GameState.
    /// </summary>
    public static class GameStateExtensions
    {
        /// <summary>
        /// If the game finished.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static bool IsGameFinished(this GameState game)
        {
            return game.TeamScores.Any(teamScore => teamScore.Score >= game.GameRules.WinningScore);
        }

        /// <summary>
        /// Get the current round.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static RoundState GetCurrentRound(this GameState game)
        {
            return game.Rounds.MaxBy(round => round.Round);
        }

        /// <summary>
        /// Get the next player after the given playerId in the rotation.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static int GetNextPlayerFrom(this GameState gameState, int playerId)
        {
            var currentPlayer = gameState.PlayerIds.Single(id => id == playerId);
            return gameState.PlayerIds.NextOf(currentPlayer);
        }
    }
}
