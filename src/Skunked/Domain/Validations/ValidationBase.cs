
namespace Skunked;

/// <summary>
/// Base class for validating cribbage commands.
/// </summary>
public abstract class ValidationBase
{
    /// <summary>
    /// Validate common elements of commands.
    /// </summary>
    /// <param name="gameState">Current game state.</param>
    /// <param name="playerId">The id of the player.</param>
    /// <param name="round">Round number.</param>
    protected void ValidateCore(GameState gameState, int playerId, int round)
    {
        CheckEndOfGame(gameState);
        if (gameState.PlayerIds.All(id => id == playerId))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidPlayer);
        }

        if (gameState.Rounds.Count(r => r.Round == round) != 1)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidRequest);
        }
    }

    /// <summary>
    /// Throw if game is complete..
    /// </summary>
    /// <param name="gameState">Current state of the game.</param>
    protected void CheckEndOfGame(GameState gameState)
    {
        if (gameState.IsGameFinished())
        {
            if (gameState.CompletedAt == null)
            {
                gameState.CompletedAt = DateTimeOffset.Now;
                gameState.GetCurrentRound().Complete = true;
            }

            throw new GameFinishedException();
        }
    }
}