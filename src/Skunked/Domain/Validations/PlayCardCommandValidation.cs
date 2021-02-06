using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Domain.Commands;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    /// <summary>
    /// Validates <see cref="PlayCardCommand"/> command.
    /// </summary>
    public class PlayCardCommandValidation : ValidationBase, IValidation<PlayCardCommand>
    {
        private readonly ScoreCalculator _scoreCalculator = new ScoreCalculator();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayCardCommandValidation"/> class.
        /// </summary>
        public PlayCardCommandValidation()
        {
        }

        /// <inheritdoc />
        public void Validate(GameState gameState, PlayCardCommand command)
        {
            var currentRound = gameState.GetCurrentRound();

            ValidateCore(gameState, command.PlayerId, currentRound.Round);
            var setOfPlays = currentRound.ThePlay;

            if (!currentRound.ThrowCardsComplete || currentRound.PlayedCardsComplete) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForPlay); }

            var playersCards = currentRound.Hands.Single(ph => ph.PlayerId == command.PlayerId).Hand.ToList();
            if (playersCards.SingleOrDefault(card => card.Equals(command.Card)) == null)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
            }

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            if (playedCards.Any(c => c.Equals(command.Card))) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardHasBeenPlayed); }
            if (!setOfPlays.Any()) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForPlay); }

            if (setOfPlays.Count == 1 && !setOfPlays.Last().Any())
            {
                if (gameState.GetNextPlayerFrom(currentRound.PlayerCrib) != command.PlayerId)
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
                }
            }

            if (setOfPlays.Last().Count > 0 && setOfPlays.SelectMany(s => s).Last().NextPlayer != command.PlayerId)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
            }

            // is the player starting new round with card sum over 31 and they have a playable card for current round?
            var currentPlayCount = _scoreCalculator.SumValues(setOfPlays.Last().Select(scs => scs.Card));
            int playCount = currentPlayCount + _scoreCalculator.SumValues(new List<Card> { new (command.Card) });
            if (playCount > GameRules.Points.MaxPlayCount)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => ppi.Card).ToList();
                var playersCardsLeftToPlay = playersCards.Except(playedCardsThisRound).Except(new List<Card> { command.Card });
                if (playersCardsLeftToPlay.Any(c => _scoreCalculator.SumValues(new List<Card>(playedCardsThisRound) { c }) <= GameRules.Points.MaxPlayCount))
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
                }
            }
        }
    }
}
