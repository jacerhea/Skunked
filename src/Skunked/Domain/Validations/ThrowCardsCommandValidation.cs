namespace Skunked;


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

        if (currentRound.Hands.Any(hand => hand.PlayerId == command.PlayerId))
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardsHaveBeenThrown);

        var dealtCards = currentRound.DealtCards.Single(hand => hand.PlayerId == command.PlayerId).Hand;
        var cards = command.CribCards.ToList();
        var requiredCount = gameState.GameRules.GetDealSize(gameState.PlayerIds.Count) - GameRules.HandSize;
        if (cards.Count != requiredCount || cards.Distinct().Count() != requiredCount ||
            cards.Any(card => !dealtCards.Contains(card)))
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
    }
}
