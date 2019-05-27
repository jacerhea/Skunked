using System;

namespace Skunked.Exceptions
{
    public class InvalidCribbageOperationException : InvalidOperationException
    {
        public InvalidCribbageOperations Operation { get; }

        public InvalidCribbageOperationException(InvalidCribbageOperations operation)
            : base(operation.ToString())
        {
            Operation = operation;
        }
    }
}
