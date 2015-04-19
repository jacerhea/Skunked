using System;

namespace Skunked.Exceptions
{
    [Serializable]
    public class InvalidCribbageOperationException : InvalidOperationException
    {
        public InvalidCribbageOperations Operation { get; private set; }

        public InvalidCribbageOperationException(InvalidCribbageOperations operation)
            : base(operation.ToString())
        {
            Operation = operation;
        }
    }
}
