using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Dealer;
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
        private readonly EventStream _eventStream;
        private readonly IPlayerHandFactory _dealer = new StandardHandDealer();
        private readonly Deck _deck = new Deck();

        public Cribbage(IEnumerable<int> players, GameRules rules)
        {
            State = new GameState { Id = Guid.NewGuid() };
            _deck.Shuffle(3);
            _eventStream = new EventStream(new List<IEventListener> { new GameStateEventListener(State, new GameStateBuilder()) })
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

        public Cribbage(GameState state)
        {
            State = state;
            var gameStateEventListener = new GameStateEventListener(State, new GameStateBuilder());
            _eventStream = new EventStream(new List<IEventListener> { gameStateEventListener });
        }

        public GameState State { get; }

        public void CutCard(int playerId, Card card)
        {
            var validation = new CardCutEventValidation();
            var cardCutEvent = new CardCutEvent { CutCard = card, PlayerId = playerId };
            validation.Validate(State, cardCutEvent);

            _eventStream.Add(cardCutEvent);

            if (State.OpeningRound.Complete)
            {
                _eventStream.Add(new RoundStartedEvent { GameId = State.Id });
                _deck.Shuffle();
                _eventStream.Add(new DeckShuffledEvent { Deck = _deck.ToList(), GameId = State.Id });
                var playerHands = _dealer.CreatePlayerHands(_deck, State.PlayerIds, State.PlayerIds.NextOf(playerId), State.GameRules.HandSizeToDeal); // get next player from who won cut.
                _eventStream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var validation = new CardsThrownEventValidation();
            var @event = new CardsThrownEvent { GameId = State.Id, Thrown = cribCards.ToList(), PlayerId = playerId, };
            validation.Validate(State, @event);

            _eventStream.Add(@event);
            if (State.GetCurrentRound().ThrowCardsComplete)
            {
                _eventStream.Add(new PlayStartedEvent { GameId = State.Id, Round = State.GetCurrentRound().Round });
            }
        }

        public void PlayCard(int playerId, Card card)
        {
            var validation = new CardPlayedEventValidation();
            var @event = new CardPlayedEvent { GameId = State.Id, Played = card, PlayerId = playerId, };
            validation.Validate(State, @event);
            _eventStream.Add(@event);

            if (State.GetCurrentRound().PlayedCardsComplete)
            {
                _eventStream.Add(new PlayFinishedEvent { GameId = State.Id, Round = State.GetCurrentRound().Round });
            }
        }

        public void CountHand(int playerId, int score)
        {
            var validation = new HandCountedEventValidation();
            var @event = new HandCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            _eventStream.Add(@event);
        }

        public void CountCrib(int playerId, int score)
        {
            var validation = new CribCountedEventValidation();
            var @event = new CribCountedEvent { GameId = State.Id, PlayerId = playerId, CountedScore = score };
            validation.Validate(State, @event);
            _eventStream.Add(@event);

            _eventStream.Add(new RoundStartedEvent { GameId = State.Id });
            _deck.Shuffle(3);
            _eventStream.Add(new DeckShuffledEvent { Deck = _deck.ToList(), GameId = State.Id });
            var playerHands = _dealer.CreatePlayerHands(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(playerId)), State.GameRules.HandSizeToDeal);
            _eventStream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
        }
    }
}
