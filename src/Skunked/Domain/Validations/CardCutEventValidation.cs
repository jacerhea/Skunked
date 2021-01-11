using System.Linq;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Exceptions;

namespace Skunked.Domain.Validations
{
    public class CardCutEventValidation : ValidationBase, IValidation<CardCutEvent>
    {
        public void Validate(GameState gameState, CardCutEvent cutEvent)
        {
            CheckEndOfGame(gameState);
            if (gameState.OpeningRound.WinningPlayerCut.HasValue)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardCardAlreadyCut);
            }

            if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Player == cutEvent.PlayerId))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardPlayerAlreadyCut);
            }

            if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Card.Equals(cutEvent.CutCard)))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardCardAlreadyCut);
            }
        }
    }
}
