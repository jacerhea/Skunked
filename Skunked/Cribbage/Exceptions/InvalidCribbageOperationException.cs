namespace Skunked.Exceptions
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
