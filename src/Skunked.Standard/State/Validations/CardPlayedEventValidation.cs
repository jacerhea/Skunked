using System.Collections.Generic;
using System.Linq;
using Skunked.Exceptions;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State.Validations
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

            if (!currentRound.ThrowCardsComplete || currentRound.PlayedCardsComplete) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForPlay); }

            var allPlayerCards = currentRound.Hands.Single(ph => ph.Id == cardPlayedEvent.PlayerId).Hand.ToList();
            if (allPlayerCards.Count(card => card.Equals(cardPlayedEvent.Played)) != 1)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            if (playedCards.Any(c => c.Equals(cardPlayedEvent.Played))) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardHasBeenPlayed); }
            if (!setOfPlays.Any()) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForPlay); }

            if (setOfPlays.Count == 1 && !setOfPlays.Last().Any())
            {
                if (gameState.GetNextPlayerFrom(currentRound.PlayerCrib) != cardPlayedEvent.PlayerId)
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
                }
            }

            if (setOfPlays.Last().Count > 0 && setOfPlays.SelectMany(s => s).Last().NextPlayer != cardPlayedEvent.PlayerId)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }

            //is the player starting new round with card sum over 31 and they have a playable card for current round?
            var currentPlayCount = _scoreCalculator.SumValues(setOfPlays.Last().Select(scs => scs.Card));
            int playCount = currentPlayCount + _scoreCalculator.SumValues(new List<Card> { new Card(cardPlayedEvent.Played) });
            if (playCount > GameRules.PlayMaxScore)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => ppi.Card).ToList();
                var playersCardsLeftToPlay = allPlayerCards.Except(playedCardsThisRound, CardValueEquality.Instance).Except(new List<Card> { cardPlayedEvent.Played }, CardValueEquality.Instance);
                if (playersCardsLeftToPlay.Any(c => _scoreCalculator.SumValues(new List<Card>(playedCardsThisRound) { c }) <= GameRules.PlayMaxScore))
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
                }
            }
        }
    }
}
