namespace Skunked.State.Events
{
    public interface IEventListener
    {
        void Notify(StreamEvent @event);
    }
}