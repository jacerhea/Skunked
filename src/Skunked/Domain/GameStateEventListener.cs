using Skunked.Domain.Events;
using Skunked.Domain.State;

namespace Skunked.Domain
{
    public class GameStateEventListener : IEventListener
    {
        private readonly GameState _gameState;
        private readonly GameStateBuilder _gameStateBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameStateEventListener"/> class.
        /// </summary>
        /// <param name="gameState">The game state.</param>
        /// <param name="gameStateBuilder"></param>
        public GameStateEventListener(GameState gameState, GameStateBuilder gameStateBuilder)
        {
            _gameState = gameState;
            _gameStateBuilder = gameStateBuilder;
        }

        public void Notify(StreamEvent @event)
        {

            var type = @event.GetType();
            if (type == typeof(GameStartedEvent))
            {
                var newGame = (GameStartedEvent)@event;
                _gameStateBuilder.Handle(newGame, _gameState);
            }
            if (type == typeof(RoundStartedEvent))
            {
                var roundStartedEvent = (RoundStartedEvent)@event;
                _gameStateBuilder.Handle(roundStartedEvent, _gameState);
            }
            if (type == typeof(DeckShuffledEvent))
            {
                var deckShuffledEvent = (DeckShuffledEvent)@event;
                _gameStateBuilder.Handle(deckShuffledEvent, _gameState);
            }
            if (type == typeof(CardCutEvent))
            {
                var cardCutEvent = (CardCutEvent)@event;
                _gameStateBuilder.Handle(cardCutEvent, _gameState);
            }
            if (type == typeof(CardsThrownEvent))
            {
                var cardsThrownEvent = (CardsThrownEvent)@event;
                _gameStateBuilder.Handle(cardsThrownEvent, _gameState);
            }
            if (type == typeof(HandsDealtEvent))
            {
                var handsDealtEvent = (HandsDealtEvent)@event;
                _gameStateBuilder.Handle(handsDealtEvent, _gameState);
            }
            if (type == typeof(StarterCardSelectedEvent))
            {
                var starterCardSelected = (StarterCardSelectedEvent)@event;
                _gameStateBuilder.Handle(starterCardSelected, _gameState);
            }
            if (type == typeof(CardPlayedEvent))
            {
                var cardPlayedEvent = (CardPlayedEvent)@event;
                _gameStateBuilder.Handle(cardPlayedEvent, _gameState);
            }
            if (type == typeof(HandCountedEvent))
            {
                var cardPlayedEvent = (HandCountedEvent)@event;
                _gameStateBuilder.Handle(cardPlayedEvent, _gameState);
            }
            if (type == typeof(CribCountedEvent))
            {
                var cardPlayedEvent = (CribCountedEvent)@event;
                _gameStateBuilder.Handle(cardPlayedEvent, _gameState);
            }
        }
    }
}