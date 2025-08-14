
namespace Skunked;


/// <summary>
/// Validates <see cref="CutCardCommand"/> command.
/// </summary>
public sealed class CutCardCommandValidation : ValidationBase, IValidation<CutCardCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CutCardCommandValidation"/> class.
    /// </summary>
    public CutCardCommandValidation()
    {
    }

    /// <inheritdoc />
    public void Validate(GameState gameState, CutCardCommand command)
    {
        CheckEndOfGame(gameState);
        if (gameState.OpeningRound.WinningPlayerCut.HasValue)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardCardAlreadyCut);
        }

        if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Player == command.PlayerId))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardPlayerAlreadyCut);
        }

        if (gameState.OpeningRound.CutCards.Any(playerCard => playerCard.Card.Equals(command.CutCard)))
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.CutCardCardAlreadyCut);
        }
    }
}