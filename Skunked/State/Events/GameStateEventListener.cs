using System;
using Skunked.Commands;

namespace Skunked.State.Events
{
    public class GameStateEventListener : IEventListener
    {
        private readonly GameState _gameState;

        public GameStateEventListener(GameState gameState)
        {
            _gameState = gameState;
        }

        public void Notify(Event @event)
        {
            var type = @event.GetType();
            if (type == typeof(NewGameStartedEvent))
            {
                var newGame = ((NewGameStartedEvent)@event);
                var command = new CreateNewCribbageGameCommand(_gameState, newGame.Players, newGame.Rules);
                command.Execute();
            }
            //if (type == typeof (DeckShuffledEvent))
            //{
            //    var shuffled = ((DeckShuffledEvent)@event);
            //    var command = new ShuffleDeckCommand(new ShuffleDeckCommandArgs(_gameState, _gameState.Rounds.Count, shuffled.PostShuffleDeck));
            //}
        }
    }
}