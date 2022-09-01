namespace Skunked.Exceptions;

/// <summary>
/// An invalid move was attempted in the game.
/// </summary>
public class InvalidCribbageOperationException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCribbageOperationException"/> class.
    /// </summary>
    /// <param name="operation">The type of operation attempted.</param>
    public InvalidCribbageOperationException(InvalidCribbageOperation operation)
        : base(operation.ToString())
    {
        Operation = operation;
    }

    /// <summary>
    /// Gets the invalid cribbage operation that was attempted.
    /// </summary>
    public InvalidCribbageOperation Operation { get; }
}