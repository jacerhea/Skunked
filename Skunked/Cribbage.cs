using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Dealer;
using Skunked.Exceptions;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;
using Skunked.State.Validations;
using Skunked.Utility;

namespace Skunked
{
    public class Cribbage
    {
        private readonly IPlayerHandFactory _dealer = new StandardHandDealer();
        private readonly Deck _deck = new Deck();

        public Cribbage(IEnumerable<int> players, GameRules rules, IEnumerable<IEventListener> eventListeners = null)
        {
            State = new GameState();
            var listeners = new List<IEventListener>(eventListeners ?? new List<IEventListener>()) { new GameStateEventListener(State, new GameStateBuilder()) };
            Stream = new EventStream(listeners)
            {
                new GameStartedEvent
                {
                    GameId = Guid.NewGuid(),
                    Occurred = DateTimeOffset.Now,
                    Rules = rules,
                    Players = players.ToList()
                },
                new DeckShuffledEvent {Deck = _deck.ToList(), GameId = State.Id}
            };
        }

        public Cribbage(EventStream eventStream, List<IEventListener> eventListeners = null)
        {
            var builder = new GameStateBuilder();
            var state = builder.Build(eventStream);
            var gameStateEventListener = new GameStateEventListener(state, builder);
            Stream = new EventStream(new List<IEventListener>(eventListeners ?? new List<IEventListener>()) { gameStateEventListener });
            State = state;
        }

        public GameState State { get; }
        public EventStream Stream { get; }

        private void AddToStream(StreamEvent @event)
        {
            try
            {
                Stream.Add(@event);
            }
            catch (InvalidCribbageOperationException e)
            {
                if (e.Operation == InvalidCribbageOperations.GameFinished)
                {
                    
                }
            }
        }

        public void CutCard(int playerId, Card card)
        {
            var validation = new CardCutEventValidation();
            var cardCutEvent = new CardCutEvent { CutCard = card, PlayerId = playerId };
            validation.Validate(State, cardCutEvent);

            Stream.Add(cardCutEvent);

            if (State.OpeningRound.Complete)
            {
                Stream.Add(new RoundStartedEvent { GameId = State.Id });
                _deck.Shuffle();
                Stream.Add(new DeckShuffledEvent { Deck = _deck.ToList(), GameId = State.Id });
                var playerHands = _dealer.CreatePlayerHands(_deck, State.PlayerIds, State.PlayerIds.NextOf(playerId), State.GameRules.HandSizeToDeal); // get next player from who won cut.
                Stream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var validation = new CardsThrownEventValidation();
            var @event = new CardsThrownEvent { GameId = State.Id, Thrown = cribCards.ToList(), PlayerId = playerId, };
            validation.Validate(State, @event);

            Stream.Add(@event);
            var currentRound = State.GetCurrentRound();
            if (currentRound.ThrowCardsComplete)
            {
                var cardsNotDealt = _deck.Except(currentRound.Crib, CardValueEquality.Instance).Except(currentRound.Hands.SelectMany(s => s.Hand), CardValueEquality.Instance).ToList();
                var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsNotDealt.Count - 1);
                var startingCard = cardsNotDealt[randomIndex];
                Stream.Add(new StarterCardSelectedEvent { GameId = State.Id, Starter = startingCard });
                Stream.Add(new PlayStartedEvent { GameId = State.Id, Round = currentRound.Round });
            }
        }

        public void PlayCard(int playerId, Card card)
        {
            var validation = new CardPlayedEventValidation();
            var @event = new CardPlayedEvent { GameId = State.Id, Played = card, PlayerId = playerId, };
            validation.Validate(State, @event);
            Stream.Add(@event);

            if (State.GetCurrentRound().PlayedCardsComplete)
            {
                Stream.Add(new PlayFinishedEvent { GameId = State.Id, Round = State.GetCurrentRound().Round });
            }
        }

        public void CountHand(int playerId, int score)
        {
            var validation = new HandCountedEventValidation();
            var @event = new HandCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            Stream.Add(@event);
        }

        public void CountCrib(int playerId, int score)
        {
            var validation = new CribCountedEventValidation();
            var @event = new CribCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            Stream.Add(@event);

            Stream.Add(new RoundStartedEvent { GameId = State.Id });
            _deck.Shuffle(3);
            Stream.Add(new DeckShuffledEvent { Deck = _deck.ToList(), GameId = State.Id });
            var playerHands = _dealer.CreatePlayerHands(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(playerId)), State.GameRules.HandSizeToDeal);
            Stream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
        }
    }
}
