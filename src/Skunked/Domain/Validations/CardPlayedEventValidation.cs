using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Exceptions;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Domain.Validations
{
    public class CardPlayedEventValidation : ValidationBase, IValidation<CardPlayedEvent>
    {
        private readonly ScoreCalculator _scoreCalculator;

        public CardPlayedEventValidation()
        {
            _scoreCalculator = new ScoreCalculator();
        }

        public void Validate(GameState gameState, CardPlayedEvent cardPlayedEvent)
        {
            var currentRound = gameState.GetCurrentRound();

            ValidateCore(gameState, cardPlayedEvent.PlayerId, currentRound.Round);
            var setOfPlays = currentRound.ThePlay;

            if (!currentRound.ThrowCardsComplete || currentRound.PlayedCardsComplete) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForPlay); }

            var playersCards = currentRound.Hands.Single(ph => ph.PlayerId == cardPlayedEvent.PlayerId).Hand.ToList();
            if (playersCards.SingleOrDefault(card => card.Equals(cardPlayedEvent.Played)) == null)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
            }

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            if (playedCards.Any(c => c.Equals(cardPlayedEvent.Played))) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.CardHasBeenPlayed); }
            if (!setOfPlays.Any()) { throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidStateForPlay); }

            if (setOfPlays.Count == 1 && !setOfPlays.Last().Any())
            {
                if (gameState.GetNextPlayerFrom(currentRound.PlayerCrib) != cardPlayedEvent.PlayerId)
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
                }
            }

            if (setOfPlays.Last().Count > 0 && setOfPlays.SelectMany(s => s).Last().NextPlayer != cardPlayedEvent.PlayerId)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperation.NotPlayersTurn);
            }

            //is the player starting new round with card sum over 31 and they have a playable card for current round?
            var currentPlayCount = _scoreCalculator.SumValues(setOfPlays.Last().Select(scs => scs.Card));
            int playCount = currentPlayCount + _scoreCalculator.SumValues(new List<Card> { new(cardPlayedEvent.Played) });
            if (playCount > GameRules.Points.MaxPlayCount)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => ppi.Card).ToList();
                var playersCardsLeftToPlay = playersCards.Except(playedCardsThisRound).Except(new List<Card> { cardPlayedEvent.Played });
                if (playersCardsLeftToPlay.Any(c => _scoreCalculator.SumValues(new List<Card>(playedCardsThisRound) { c }) <= GameRules.Points.MaxPlayCount))
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperation.InvalidCard);
                }
            }
        }
    }
}
