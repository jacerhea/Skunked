using System.Collections.Generic;
using System.Linq;
using Skunked.Exceptions;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State.Validations
{
    public class CardsThrownEventValidation : ValidationBase, IValidation<CardsThrownEvent>
    {
        public void Validate(GameState gameState, CardsThrownEvent cutEvent)
        {
            var currentRound = gameState.GetCurrentRound();
            ValidateCore(gameState, cutEvent.PlayerId, currentRound.Round);
            if (currentRound.ThrowCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardsHaveBeenThrown);
            }

            var dealtCards = currentRound.DealtCards.Single(playerHand => playerHand.Id == cutEvent.PlayerId).Hand;

            if (dealtCards.Intersect(cutEvent.Thrown).Count() != cutEvent.Thrown.Count())
            {
                //invalid request, player was not dealt these cards
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            var cardsAlreadyThrownToCrib = dealtCards.Intersect(currentRound.Crib).Count();
            var twoPlayer = new List<int> { 2 };
            var threeOrFourPlayer = new List<int> { 3, 4 };
            if (cardsAlreadyThrownToCrib == 1 && threeOrFourPlayer.Contains(gameState.GameRules.PlayerCount))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardsHaveBeenThrown);
            }

            if (cardsAlreadyThrownToCrib == 2 && twoPlayer.Contains(gameState.GameRules.PlayerCount))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardsHaveBeenThrown);
            }
        }
    }
}
