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
                var command = new CreateNewCribbageGameCommand(newGame.Players, _gameState, newGame.Rules);
                command.Execute();

            }
            else if (type == typeof(PlayCardEvent))
            {
                var playCardEvent = ((PlayCardEvent)@event);
                var command = new PlayCardCommand(new PlayCardArgs(_gameState, playCardEvent.PlayerId, _gameState.Rounds.Count, playCardEvent.PlayedCard));
                command.Execute();

            }
            else if (type == typeof(CutCardEvent))
            {
                var cutCardEvent = ((CutCardEvent)@event);
                var command = new CutCardCommand(new CutCardArgs(_gameState, cutCardEvent.PlayerId, _gameState.Rounds.Count, cutCardEvent.CutCard));
                command.Execute();

                if (_gameState.OpeningRound.Complete)
                {
                    var newRoundCommand = new CreateNewRoundCommand(_gameState, 0);
                    newRoundCommand.Execute();
                }
            }
            else if (type == typeof(ThrowCardsEvent))
            {
                var throwCardsEvent = ((ThrowCardsEvent)@event);
                var command = new ThrowCardsToCribCommand(new ThrowCardsToCribArgs(_gameState, throwCardsEvent.PlayerId, _gameState.Rounds.Count, throwCardsEvent.Thrown));
                command.Execute();
            }
            else if (type == typeof(CountHandEvent))
            {
                var countHand = ((CountHandEvent)@event);
                var command = new CountHandScoreCommand(new CountHandScoreArgs(_gameState, countHand.PlayerId, _gameState.Rounds.Count, countHand.CountedScore));
                command.Execute();
            }
            else if (type == typeof(CountCribEvent))
            {
                var cribCount = ((CountCribEvent)@event);
                var command = new CountCribScoreCommand(new CountCribScoreArgs(_gameState, cribCount.PlayerId, _gameState.Rounds.Count, cribCount.CountedScore));
                command.Execute();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}