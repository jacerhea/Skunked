using Games.Domain.MainModule.Entities.CardGames.Exceptions;

namespace Cribbage.Exceptions
{
    public class InvalidCribbageOperationException : InvalidGamePlayOperation
    {
        public InvalidCribbageOperations Operation { get; private set; }

        public InvalidCribbageOperationException(InvalidCribbageOperations operation)
            : base(operation.ToString())
        {
            Operation = operation;
        }
    }
}
