using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked
{
    public class Cribbage
    {
        private readonly GameEventStream _eventStream = new GameEventStream();
        private readonly IEventListener _gameStateEventListener;

        public Cribbage(IEnumerable<int> players, GameRules rules)
        {
            State = new GameState();
            _gameStateEventListener = new GameStateEventListener(State);

            _gameStateEventListener.Notify(new NewGameStartedEvent{Players = players.ToList(), Rules = rules});
            _gameStateEventListener.Notify(new DeckShuffledEvent());
        }

        public Cribbage(GameState state)
        {
            State = state;
            _gameStateEventListener = new GameStateEventListener(State);
        }

        public GameState State { get; private set; }

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
