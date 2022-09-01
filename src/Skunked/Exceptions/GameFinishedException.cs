namespace Skunked.Exceptions;

/// <summary>
/// An exception to stop the game.
/// </summary>
public class GameFinishedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameFinishedException"/> class.
    /// </summary>
    public GameFinishedException() { }
}