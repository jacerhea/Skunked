using System;

namespace Skunked.Exceptions
{
    /// <summary>
    /// An invalid move was attempted in the game.
    /// </summary>
    public class InvalidCribbageOperationException : InvalidOperationException
    {
        /// <summary>
        /// The invalid cribbage operation that was attempted.
        /// </summary>
        public InvalidCribbageOperation Operation { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        public InvalidCribbageOperationException(InvalidCribbageOperation operation)
            : base(operation.ToString())
        {
            Operation = operation;
        }
    }
}
