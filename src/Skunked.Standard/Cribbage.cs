using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Dealer;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;
using Skunked.State.Validations;
using Skunked.Utility;

namespace Skunked
{
    /// <summary>
    /// The class for creating and modeling a game of Cribbage. "Plays" are validated and stored as a stream of events. Event listeners allow denormalized state to be created from the stream.
    /// </summary>
    public class Cribbage
    {
        private readonly IDealer _dealer = new StandardDealer();
        private readonly Deck _deck = new Deck();

        /// <summary>
        /// Constructs a new game of Cribbage.
        /// </summary>
        /// <param name="players"></param>
        /// <param name="rules"></param>
        /// <param name="eventListeners"></param>
        public Cribbage(IEnumerable<int> players, GameRules rules = null, IEnumerable<IEventListener> eventListeners = null)
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
                new DeckShuffledEvent {GameId = State.Id, Deck = _deck.ToList()}
            };
        }

        public GameState State { get; }
        public EventStream Stream { get; }

        public void CutCard(int playerId, Card card)
        {
            var validation = new CardCutEventValidation();
            var cardCutEvent = new CardCutEvent { GameId = State.Id, CutCard = card, PlayerId = playerId };
            validation.Validate(State, cardCutEvent);

            Stream.Add(cardCutEvent);

            if (State.OpeningRound.Complete)
            {
                Stream.Add(new RoundStartedEvent { GameId = State.Id });
                _deck.Shuffle();
                Stream.Add(new DeckShuffledEvent { GameId = State.Id, Deck = _deck.ToList() });
                var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(playerId), State.GameRules.HandSizeToDeal); // get next player from who won cut.
                Stream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var validation = new CardsThrownEventValidation();
            var @event = new CardsThrownEvent { GameId = State.Id, Thrown = cribCards.ToList(), PlayerId = playerId };
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
            var @event = new CardPlayedEvent { GameId = State.Id, Played = card, PlayerId = playerId };
            validation.Validate(State, @event);
            Stream.Add(@event);

            if (State.GetCurrentRound().PlayedCardsComplete)
            {
                Stream.Add(new PlayFinishedEvent { GameId = State.Id, Round = State.GetCurrentRound().Round });
            }
        }

        public void CountHand(int playerId, int score)
        {
            var validation = new HandCountedEventValidation(new ScoreCalculator());
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
            Stream.Add(new DeckShuffledEvent { GameId = State.Id, Deck = _deck.ToList() });
            var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(playerId)), State.GameRules.HandSizeToDeal);
            Stream.Add(new HandsDealtEvent { GameId = State.Id, Hands = playerHands });
        }
    }
}
