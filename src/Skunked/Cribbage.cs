using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Domain;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Domain.Validations;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked
{
    /// <summary>
    /// The class for creating and modeling a game of Cribbage. "Plays" are validated and stored as a stream of events. Event listeners allow denormalized state to be created from the stream.
    /// </summary>
    public class Cribbage
    {
        private readonly Dealer _dealer = new ();
        private readonly Deck _deck = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="Cribbage"/> class.
        /// </summary>
        /// <param name="players"></param>
        /// <param name="rules"></param>
        /// <param name="eventListeners"></param>
        public Cribbage(IEnumerable<int> players, GameRules rules, IEnumerable<IEventListener>? eventListeners = null)
        {
            State = new GameState();
            var internalStateBuilder = new GameStateEventListener(State, new GameStateBuilder());
            var listeners = new List<IEventListener> { internalStateBuilder };
            listeners.AddRange(eventListeners ?? Enumerable.Empty<IEventListener>());
            Stream = new EventStream(listeners);

            Emit(new GameStartedEvent(Guid.NewGuid(), NewVersion, players.ToList(), rules));
            Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
        }


        /// <summary>
        /// Gets the state of the game.
        /// </summary>
        public GameState State { get; }

        /// <summary>
        /// Gets set of events that have occurred in the game.
        /// </summary>
        public EventStream Stream { get; }

        private int NewVersion => State.Version + 1;

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="card"></param>
        public void CutCard(int playerId, Card card)
        {
            var validation = new CardCutEventValidation();
            var cardCutEvent = new CardCutEvent(State.Id, 1, playerId, card);
            validation.Validate(State, cardCutEvent);

            Emit(cardCutEvent);

            if (State.OpeningRound.Complete)
            {
                Emit(new RoundStartedEvent(State.Id, NewVersion));
                _deck.Shuffle();
                Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
                var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(playerId), State.GameRules.DealSize); // get next player from who won cut.
                Emit(new HandsDealtEvent(State.Id, NewVersion, playerHands));
            }
        }

        /// <summary>
        /// Throw cards to the crib.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="cribCards"></param>
        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var validation = new CardsThrownEventValidation();
            var cardsThrown = new CardsThrownEvent(State.Id, NewVersion, playerId, cribCards.ToList());
            validation.Validate(State, cardsThrown);

            Emit(cardsThrown);
            var currentRound = State.GetCurrentRound();
            if (currentRound.ThrowCardsComplete)
            {
                var cardsNotDealt = _deck.Except(currentRound.Crib).Except(currentRound.Hands.SelectMany(s => s.Hand)).ToList();
                var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsNotDealt.Count - 1);
                var startingCard = cardsNotDealt[randomIndex];
                Emit(new StarterCardSelectedEvent(State.Id, NewVersion, startingCard));
                Emit(new PlayStartedEvent(State.Id, NewVersion, currentRound.Round));
            }
        }

        /// <summary>
        /// Play a card.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="card"></param>
        public void PlayCard(int playerId, Card card)
        {
            var validation = new CardPlayedEventValidation();
            var @event = new CardPlayedEvent(State.Id, NewVersion, playerId, card);
            validation.Validate(State, @event);
            Emit(@event);

            if (State.GetCurrentRound().PlayedCardsComplete)
            {
                Emit(new PlayFinishedEvent(State.Id, NewVersion, State.GetCurrentRound().Round));
            }
        }

        /// <summary>
        /// Count a players hand.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <param name="score">Score.  Over counting is penalized.</param>
        public void CountHand(int playerId, int score)
        {
            var validation = new HandCountedEventValidation(new ScoreCalculator());
            var @event = new HandCountedEvent(State.Id, NewVersion, playerId, score);
            validation.Validate(State, @event);
            Emit(@event);
        }

        /// <summary>
        /// Count a players crib.
        /// </summary>
        /// <param name="playerId">Player Id.</param>
        /// <param name="score">Score.  Over counting is penalized.</param>
        public void CountCrib(int playerId, int score)
        {
            var validation = new CribCountedEventValidation();
            var @event = new CribCountedEvent(State.Id, NewVersion, playerId, score);
            validation.Validate(State, @event);
            Emit(@event);

            Emit(new RoundStartedEvent(State.Id, NewVersion));
            _deck.Shuffle(3);
            Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
            var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(playerId)), State.GameRules.DealSize);
            Emit(new HandsDealtEvent(State.Id, NewVersion, playerHands));
        }

        private void Emit(StreamEvent @event)
        {
            Stream.Add(@event);
        }
    }
}
