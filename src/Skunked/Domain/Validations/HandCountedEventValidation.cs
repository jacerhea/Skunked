using System.Linq;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    public class HandCountedEventValidation : ValidationBase, IValidation<HandCountedEvent>
    {
        private readonly ScoreCalculator _scoreCalculator;

        public HandCountedEventValidation(ScoreCalculator scoreCalculator)
        {
            _scoreCalculator = scoreCalculator;
        }

        public void Validate(GameState gameState, HandCountedEvent handCountedEvent)
        {
            var currentRound = gameState.GetCurrentRound();

            ValidateCore(gameState, handCountedEvent.PlayerId, currentRound.Round);
            if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForCount);
            }

            if (currentRound.ShowScores.Single(pss => pss.Player == handCountedEvent.PlayerId).HasShowed)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.PlayerHasAlreadyCounted);
            }

            var playersHand = currentRound.Hands.Single(hand => hand.PlayerId == handCountedEvent.PlayerId);
            var maxScorePossible = _scoreCalculator.CountShowPoints(currentRound.Starter, playersHand.Hand);
            if (handCountedEvent.CountedScore > maxScorePossible.Points.Score)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidShowCount);
            }

            var currentPlayer = gameState.PlayerIds.NextOf(gameState.PlayerIds.Single(id => id == currentRound.PlayerCrib));
            foreach (var _ in Enumerable.Range(1, gameState.PlayerIds.Count))
            {
                var playerScoreShow = currentRound.ShowScores.Single(pss => pss.Player == currentPlayer);

                if (playerScoreShow.Player == handCountedEvent.PlayerId) { break; }
                if (!playerScoreShow.HasShowed) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn); }

                currentPlayer = gameState.PlayerIds.NextOf(currentPlayer);
            }
        }
    }
}
