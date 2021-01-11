using Skunked.Domain.Events;
using Skunked.Domain.State;

namespace Skunked.Domain.Validations
{
    public interface IValidation<in T> where T : StreamEvent
    {
        void Validate(GameState gameState, T cutEvent);
    }
}