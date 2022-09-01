using Skunked.Domain.State;

namespace Skunked.Utility;

/// <summary>
/// Set of convenience methods to aggregate state from GameState.
/// </summary>
public static class GameStateExtensions
{
    /// <summary>
    /// If the game finished.
    /// </summary>
    /// <param name="game">The game state.</param>
    /// <returns>True if the game is finished and no more moves may be made.</returns>
    public static bool IsGameFinished(this GameState game)
    {
        return game.TeamScores.Any(teamScore => teamScore.Score >= game.GameRules.WinningScore);
    }

    /// <summary>
    /// Get the current round.
    /// </summary>
    /// <param name="game">The game state.</param>
    /// <returns>The current round.</returns>
    public static RoundState GetCurrentRound(this GameState game)
    {
        return game.Rounds.MaxBy(round => round.Round);
    }

    /// <summary>
    /// Get the next player after the given playerId in the rotation.
    /// </summary>
    /// <param name="gameState">The game state.</param>
    /// <param name="playerId">The id of the player.</param>
    /// <returns>The next player id in the rotation.</returns>
    public static int GetNextPlayerFrom(this GameState gameState, int playerId)
    {
        var currentPlayer = gameState.PlayerIds.Single(id => id == playerId);
        return gameState.PlayerIds.NextOf(currentPlayer);
    }
}