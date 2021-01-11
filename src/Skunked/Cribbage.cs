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
        private readonly Dealer _dealer = new();
        private readonly Deck _deck = new();

        /// <summary>
        /// Constructs a new game of Cribbage.
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

            Emit(new GameStartedEvent
            {
                GameId = Guid.NewGuid(),
                Occurred = DateTimeOffset.Now,
                Rules = rules,
                Players = players.ToList()
            });
            Emit(new DeckShuffledEvent { GameId = State.Id, Deck = _deck.ToList() });
        }

        public GameState State { get; }
        public EventStream Stream { get; }

        public void CutCard(int playerId, Card card)
        {
            var validation = new CardCutEventValidation();
            var cardCutEvent = new CardCutEvent { GameId = State.Id, CutCard = card, PlayerId = playerId };
            validation.Validate(State, cardCutEvent);

            Emit(cardCutEvent);

            if (State.OpeningRound.Complete)
            {
                Emit(new RoundStartedEvent { GameId = State.Id });
                _deck.Shuffle();
                Emit(new DeckShuffledEvent { GameId = State.Id, Deck = _deck.ToList() });
                var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(playerId), State.GameRules.DealSize); // get next player from who won cut.
                Emit(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var validation = new CardsThrownEventValidation();
            var cardsThrown = new CardsThrownEvent { GameId = State.Id, Thrown = cribCards.ToList(), PlayerId = playerId };
            validation.Validate(State, cardsThrown);

            Emit(cardsThrown);
            var currentRound = State.GetCurrentRound();
            if (currentRound.ThrowCardsComplete)
            {
                var cardsNotDealt = _deck.Except(currentRound.Crib).Except(currentRound.Hands.SelectMany(s => s.Hand)).ToList();
                var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsNotDealt.Count - 1);
                var startingCard = cardsNotDealt[randomIndex];
                Emit(new StarterCardSelectedEvent { GameId = State.Id, Starter = startingCard });
                Emit(new PlayStartedEvent { GameId = State.Id, Round = currentRound.Round });
            }
        }

        public void PlayCard(int playerId, Card card)
        {
            var validation = new CardPlayedEventValidation();
            var @event = new CardPlayedEvent { GameId = State.Id, Played = card, PlayerId = playerId };
            validation.Validate(State, @event);
            Emit(@event);

            if (State.GetCurrentRound().PlayedCardsComplete)
            {
                Emit(new PlayFinishedEvent { GameId = State.Id, Round = State.GetCurrentRound().Round });
            }
        }

        public void CountHand(int playerId, int score)
        {
            var validation = new HandCountedEventValidation(new ScoreCalculator());
            var @event = new HandCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            Emit(@event);
        }

        public void CountCrib(int playerId, int score)
        {
            var validation = new CribCountedEventValidation();
            var @event = new CribCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            Emit(@event);

            Emit(new RoundStartedEvent { GameId = State.Id });
            _deck.Shuffle(3);
            Emit(new DeckShuffledEvent { GameId = State.Id, Deck = _deck.ToList() });
            var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(playerId)), State.GameRules.DealSize);
            Emit(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
        }

        public void Emit(StreamEvent @event)
        {
            Stream.Add(@event);
        }
    }
}
