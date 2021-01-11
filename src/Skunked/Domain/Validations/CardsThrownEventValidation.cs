﻿using System.Collections.Generic;
using System.Linq;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    public class CardsThrownEventValidation : ValidationBase, IValidation<CardsThrownEvent>
    {
        public void Validate(GameState gameState, CardsThrownEvent cutEvent)
        {
            var currentRound = gameState.GetCurrentRound();
            ValidateCore(gameState, cutEvent.PlayerId, currentRound.Round);
            if (currentRound.ThrowCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
            }

            var dealtCards = currentRound.DealtCards.Single(playerHand => playerHand.PlayerId == cutEvent.PlayerId).Hand;

            if (dealtCards.Intersect(cutEvent.Thrown).Count() != cutEvent.Thrown.Count)
            {
                //invalid request, player was not dealt these cards
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
            }

            var cardsAlreadyThrownToCrib = dealtCards.Intersect(currentRound.Crib).Count();
            var twoPlayer = new List<int> { 2 };
            var threeOrFourPlayer = new List<int> { 3, 4 };
            if (cardsAlreadyThrownToCrib == 1 && threeOrFourPlayer.Contains(gameState.GameRules.NumberOfPlayers))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
            }

            if (cardsAlreadyThrownToCrib == 2 && twoPlayer.Contains(gameState.GameRules.NumberOfPlayers))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
            }
        }
    }
}