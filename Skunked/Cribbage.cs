using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked
{
    public class Cribbage
    {
        private readonly GameEventStream _stream;
        private readonly EventDispatcher _dispatcher;

        public Cribbage()
        {
            _stream = new GameEventStream();
            State = new GameState();
            _dispatcher = new EventDispatcher(new GameStateEventListener(State));
        }

        public GameState State { get; private set; }

        public void Start(IEnumerable<Player> players, GameRules rules)
        {
            var @event = new NewGameStartedEvent { Players = players.ToList(), Rules = rules };
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }

        public void CutCard(int playerId, Card card)
        {
            var @event = new CutCardEvent { PlayerId = playerId, CutCard = card };
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var @event = new ThrowCardsEvent { PlayerId = playerId, Thrown = cribCards.ToList() };
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }

        public void PlayCard(int playerId, Card card)
        {
            var @event = new PlayCardEvent { PlayerId = playerId, Occurred = DateTimeOffset.Now, PlayedCard = card };
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }

        public void CountHand(int playerId, int score)
        {
            var @event = new CountHandEvent { PlayerId = playerId, Occurred = DateTimeOffset.Now, CountedScore = score };
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }

        public void CountCrib(int playerId, int score)
        {
            var @event = new CountCribEvent{PlayerId = playerId, CountedScore = score};
            _stream.Add(@event);
            _dispatcher.RaiseEvent(@event);
        }
    }
}
