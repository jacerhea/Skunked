using System.Linq;
using Skunked.Players;
using Skunked.State;

namespace Skunked.Utility
{
    public static class GameStateExtensions
    {
        public static bool IsGameFinished(this GameState game)
        {
            return game.IndividualScores.Any(ps => ps.Score >= game.Rules.WinningScore);
        }

        public static RoundState GetCurrentRound(this GameState game)
        {
            return game.Rounds.MaxBy(round => round.Round);
        }

        public static Player GetNextPlayerFrom(this GameState gameState, int playerId)
        {
            var currentPlayer = gameState.Players.Single(p => p.Id == playerId);
            return gameState.Players.NextOf(currentPlayer);
        }
    }
}
