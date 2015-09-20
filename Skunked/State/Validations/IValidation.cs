using Skunked.State.Events;

namespace Skunked.State.Validations
{
    public interface IValidation<in T> where T : StreamEvent
    {
        void Validate(GameState gameState, T cutEvent);
    }
}