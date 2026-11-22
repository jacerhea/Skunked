
namespace Skunked;

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
        if (gameState.Rounds.Count == 0)
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidRequest);
        var currentRound = gameState.GetCurrentRound();

        ValidateCore(gameState, command.PlayerId, currentRound.Round);
        if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForCribCount);
        }

        if (command.PlayerId != currentRound.PlayerCrib || !currentRound.ShowScores.All(pss => pss.HasShowed))
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
        var actualScore = new ScoreCalculator().CountShowPoints(currentRound.Starter, currentRound.Crib, isCrib: true).Points.Score;
        if (command.Score < 0 || command.Score > actualScore)
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidShowCount);

    }
}