namespace Skunked.State.Events
{
    public class GameStateEventListener : IEventListener
    {
        private readonly GameState _gameState;
        private readonly GameStateBuilder _gameStateBuilder;

        public GameStateEventListener(GameState gameState, GameStateBuilder gameStateBuilder)
        {
            _gameState = gameState;
            _gameStateBuilder = gameStateBuilder;
        }

        public void Notify(Event @event)
        {

            var type = @event.GetType();
            if (type == typeof(GameStartedEvent))
            {
                var newGame = ((GameStartedEvent)@event);
                _gameStateBuilder.Handle(newGame, _gameState);

            }
            if (type == typeof(DeckShuffledEvent))
            {
                var deckShuffledEvent = ((DeckShuffledEvent)@event);
                _gameStateBuilder.Handle(deckShuffledEvent, _gameState);
            }
        }
    }
}