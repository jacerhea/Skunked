using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Domain.Validations;

/// <summary>
/// Validates <see cref="ThrowCardsCommand"/> command.
/// </summary>
public sealed class ThrowCardsCommandValidation : ValidationBase, IValidation<ThrowCardsCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowCardsCommandValidation"/> class.
    /// </summary>
    public ThrowCardsCommandValidation()
    {
    }

    /// <inheritdoc />
    public void Validate(GameState gameState, ThrowCardsCommand command)
    {
        var currentRound = gameState.GetCurrentRound();
        ValidateCore(gameState, command.PlayerId, currentRound.Round);
        if (currentRound.ThrowCardsComplete)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
        }

        var dealtCards = currentRound.DealtCards.Single(playerHand => playerHand.PlayerId == command.PlayerId).Hand;

        if (dealtCards.Intersect(command.CribCards).Count() != command.CribCards.Count())
        {
            // invalid request, player was not dealt these cards
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
        }

        var cardsAlreadyThrownToCrib = dealtCards.Intersect(currentRound.Crib).Count();
        var twoPlayer = new List<int> { 2 };
        var threeOrFourPlayer = new List<int> { 3, 4 };
        if (cardsAlreadyThrownToCrib == 1 &&
            threeOrFourPlayer.Contains(gameState.GameRules.GetDealSize(gameState.PlayerIds.Count)))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
        }

        if (cardsAlreadyThrownToCrib == 2 &&
            twoPlayer.Contains(gameState.GameRules.GetDealSize(gameState.PlayerIds.Count)))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);
        }
    }
}