namespace Skunked.State.Events
{
    public class EventDispatcher
    {
        private readonly GameStateEventListener _listener;

        public EventDispatcher(GameStateEventListener listener)
        {
            _listener = listener;
        }

        public void RaiseEvent(Event @event)
        {
            _listener.Notify(@event);
        }
    }
}