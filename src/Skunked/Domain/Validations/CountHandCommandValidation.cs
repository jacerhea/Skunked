using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Domain.Validations;

/// <summary>
/// Validates <see cref="CountHandCommand"/> command.
/// </summary>
public class CountHandCommandValidation : ValidationBase, IValidation<CountHandCommand>
{
    private readonly ScoreCalculator _scoreCalculator = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CountHandCommandValidation"/> class.
    /// </summary>
    public CountHandCommandValidation()
    {
    }

    /// <inheritdoc />
    public void Validate(GameState gameState, CountHandCommand command)
    {
        var currentRound = gameState.GetCurrentRound();

        ValidateCore(gameState, command.PlayerId, currentRound.Round);
        if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForCount);
        }

        if (currentRound.ShowScores.Single(pss => pss.Player == command.PlayerId).HasShowed)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.PlayerHasAlreadyCounted);
        }

        var playersHand = currentRound.Hands.Single(hand => hand.PlayerId == command.PlayerId);
        var maxScorePossible = _scoreCalculator.CountShowPoints(currentRound.Starter, playersHand.Hand);
        if (command.Score > maxScorePossible.Points.Score)
        {
            throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidShowCount);
        }

        var currentPlayer = gameState.PlayerIds.NextOf(gameState.PlayerIds.Single(id => id == currentRound.PlayerCrib));
        foreach (var _ in Enumerable.Range(1, gameState.PlayerIds.Count))
        {
            var playerScoreShow = currentRound.ShowScores.Single(pss => pss.Player == currentPlayer);

            if (playerScoreShow.Player == command.PlayerId)
            {
                break;
            }

            if (!playerScoreShow.HasShowed)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
            }

            currentPlayer = gameState.PlayerIds.NextOf(currentPlayer);
        }
    }
}