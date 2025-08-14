
namespace Skunked;


/// <summary>
/// Validate commands are valid given the current state of the game.
/// </summary>
/// <typeparam name="T">Type of command to be validated.</typeparam>
public interface IValidation<in T>
    where T : CommandBase
{
    /// <summary>
    /// Exception is thrown if command is not valid when applied to the current state of the game.
    /// </summary>
    /// <param name="gameState">The current state of the game.</param>
    /// <param name="command">The command being validated.</param>
    void Validate(GameState gameState, T command);
}