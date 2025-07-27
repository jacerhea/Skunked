namespace Skunked.Domain.Commands;

/// <summary>
/// Command to count the points in the crib.
/// </summary>
public class CountCribCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountCribCommand"/> class.
    /// </summary>
    /// <param name="playerId">The id of the player associated with the command for the crib.</param>
    /// <param name="score">The score that was counted.</param>
    public CountCribCommand(int playerId, int score)
        : base(playerId)
    {
        Score = score;
    }

    /// <summary>
    /// Gets the score that was counted for the crib.
    /// </summary>
    public int Score { get; }
}