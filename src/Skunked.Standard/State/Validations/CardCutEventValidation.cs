using System.Linq;
using Skunked.Exceptions;
using Skunked.State.Events;

namespace Skunked.State.Validations
{
    public class CardCutEventValidation : ValidationBase, IValidation<CardCutEvent>
    {
        public void Validate(GameState gameState, CardCutEvent cutEvent)
        {
            CheckEndOfGame(gameState);
            if (gameState.PlayerIds.All(id => id == cutEvent.PlayerId)) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidPlayer); }


            if (gameState.OpeningRound.CutCards.Any(kv => kv.Player == cutEvent.PlayerId))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }

            if (gameState.OpeningRound.CutCards.Any(kv => kv.Card.Equals(cutEvent.CutCard)))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }
        }
    }
}
