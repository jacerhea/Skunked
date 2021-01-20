using System;
using System.Linq;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    /// <summary>
    /// Base class for validating cribbage commands.
    /// </summary>
    public abstract class ValidationBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerId">The id of the player.</param>
        /// <param name="round"></param>
        protected void ValidateCore(GameState gameState, int playerId, int round)
        {
            CheckEndOfGame(gameState);
            if (gameState.PlayerIds.All(id => id == playerId)) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidPlayer); }

            if (gameState.Rounds.Count(r => r.Round == round) != 1)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidRequest);
            }
        }

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
}