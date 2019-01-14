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
            if (gameState.OpeningRound.WinningPlayerCut.HasValue)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardCardAlreadyCut);
            }

            if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Player == cutEvent.PlayerId))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }

            if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Card.Equals(cutEvent.CutCard)))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardCardAlreadyCut);
            }
        }
    }
}
