namespace Skunked.Domain.Events
{
    public interface IEventListener
    {
        void Notify(StreamEvent @event);
    }
}