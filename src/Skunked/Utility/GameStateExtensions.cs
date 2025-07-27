using System.Linq;
using Skunked.Domain.State;

namespace Skunked.Utility;

/// <summary>
/// Set of convenience methods to aggregate state from GameState.
/// </summary>
public static class GameStateExtensions
{
    /// <summary>
    /// Checks whether the game has finished based on the team scores and game rules.
    /// </summary>
    /// <param name="gameState">The current state of the game.</param>
    /// <returns>True if the game has finished; otherwise, false.</returns>
    public static bool IsGameFinished(this GameState gameState)
    {
        return gameState.TeamScores.Any(teamScore => teamScore.Score >= gameState.GameRules.WinningScore);
    }

    /// <summary>
    /// Get the current round.
    /// </summary>
    /// <param name="gameState">The current state of the game, containing player information.</param>
    /// <returns>The current round.</returns>
    public static RoundState GetCurrentRound(this GameState gameState)
    {
        return gameState.Rounds.MaxBy(round => round.Round) !;
    }

    /// <summary>
    /// Retrieves the ID of the next player in the rotation following the specified player.
    /// </summary>
    /// <param name="gameState">The current state of the game, containing player information.</param>
    /// <param name="playerId">The unique identifier of the current player.</param>
    /// <returns>The unique identifier of the next player in the rotation.</returns>
    public static int GetNextPlayerFrom(this GameState gameState, int playerId)
    {
        var currentPlayer = gameState.PlayerIds.Single(id => id == playerId);
        return gameState.PlayerIds.NextOf(currentPlayer);
    }
}