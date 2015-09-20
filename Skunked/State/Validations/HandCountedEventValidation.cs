using System.Linq;
using Skunked.Exceptions;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State.Validations
{
    public class HandCountedEventValidation : ValidationBase, IValidation<HandCountedEvent>
    {
        public void Validate(GameState gameState, HandCountedEvent handCountedEvent)
        {
            var currentRound = gameState.GetCurrentRound();

            ValidateCore(gameState, handCountedEvent.PlayerId, currentRound.Round);
            if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCount);
            }

            if (currentRound.ShowScores.Single(pss => pss.Player == handCountedEvent.PlayerId).HasShowed)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.PlayerHasAlreadyCounted);
            }

            var currentPlayer = gameState.PlayerIds.NextOf(gameState.PlayerIds.Single(id => id == currentRound.PlayerCrib));
            foreach (var enumeration in Enumerable.Range(1, gameState.PlayerIds.Count))
            {
                var playerScoreShow = currentRound.ShowScores.Single(pss => pss.Player == currentPlayer);

                if (playerScoreShow.Player == handCountedEvent.PlayerId) { break; }
                if (!playerScoreShow.HasShowed) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn); }

                currentPlayer = gameState.PlayerIds.NextOf(currentPlayer);
            }
        }
    }
}
