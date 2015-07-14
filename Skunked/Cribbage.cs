using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked
{
    public class Cribbage
    {
        private readonly EventStream _eventStream;

        public Cribbage(IEnumerable<int> players, GameRules rules)
        {
            State = new GameState { Id = Guid.NewGuid() };
            var deck = new Deck();
            deck.Shuffle();
            _eventStream = new EventStream(new List<IEventListener> { new GameStateEventListener(State, new GameStateBuilder()) })
            {
                new GameStartedEvent
                {
                    GameId = Guid.NewGuid(),
                    Occurred = DateTimeOffset.Now,
                    Rules = rules,
                    Players = players.ToList()
                },
                new DeckShuffledEvent {Deck = deck.ToList()}
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
            var command = new CutCardCommand(new CutCardArgs(_eventStream, State, playerId, State.Rounds.Count, card));
            command.Execute();

            if (State.OpeningRound.Complete)
            {
                var newRoundCommand = new CreateNewRoundCommand(State, 0);
                newRoundCommand.Execute();
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var command = new ThrowCardsToCribCommand(new ThrowCardsToCribArgs(State, playerId, State.Rounds.Count, cribCards));
            command.Execute();
        }

        public void PlayCard(int playerId, Card card)
        {
            var command = new PlayCardCommand(new PlayCardArgs(State, playerId, State.Rounds.Count, card));
            command.Execute();
        }

        public void CountHand(int playerId, int score)
        {
            var command = new CountHandScoreCommand(new CountHandScoreArgs(State, playerId, State.Rounds.Count, score));
            command.Execute();
        }

        public void CountCrib(int playerId, int score)
        {
            var command = new CountCribScoreCommand(new CountCribScoreArgs(State, playerId, State.Rounds.Count, score));
            command.Execute();
        }
    }
}
