using System.Linq;
using Skunked.Exceptions;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State.Validations
{
    public class CribCountedEventValidation : ValidationBase, IValidation<CribCountedEvent>
    {
        public void Validate(GameState gameState, CribCountedEvent cutEvent)
        {
            var currentRound = gameState.GetCurrentRound();

            ValidateCore(gameState, cutEvent.PlayerId, currentRound.Round);
            if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForCribCount);
            }

            if (!currentRound.ShowScores.Where(pss => pss.Player != cutEvent.PlayerId).All(pss => pss.HasShowed))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
            }
        }
    }
}
