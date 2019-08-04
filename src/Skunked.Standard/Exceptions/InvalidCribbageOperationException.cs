using System;

namespace Skunked.Exceptions
{
    public class InvalidCribbageOperationException : InvalidOperationException
    {
        public InvalidCribbageOperation Operation { get; }

        public InvalidCribbageOperationException(InvalidCribbageOperation operation)
            : base(operation.ToString())
        {
            Operation = operation;
        }
    }
}
