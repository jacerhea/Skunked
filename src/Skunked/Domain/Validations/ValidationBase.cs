using System;
using System.Linq;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    public abstract class ValidationBase
    {
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
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.GameFinished);
            }
        }
    }
}