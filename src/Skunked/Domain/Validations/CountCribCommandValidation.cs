using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Domain.Validations;

/// <summary>
/// Validates <see cref="CountCribCommand"/> command.
/// </summary>
public sealed class CountCribCommandValidation : ValidationBase, IValidation<CountCribCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountCribCommandValidation"/> class.
    /// </summary>
    public CountCribCommandValidation()
    {
    }

    /// <inheritdoc />
    public void Validate(GameState gameState, CountCribCommand command)
    {
        var currentRound = gameState.GetCurrentRound();

        ValidateCore(gameState, command.PlayerId, currentRound.Round);
        if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForCribCount);
        }

        if (!currentRound.ShowScores.Where(pss => pss.Player != command.PlayerId).All(pss => pss.HasShowed))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
        }
    }
}