namespace Skunked.Domain.Commands
{
    /// <summary>
    /// Command to count the points in a hand.
    /// </summary>
    public class CountHandCommand : CommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountHandCommand"/> class.
        /// </summary>
        /// <param name="playerId">Id of the player.</param>
        /// <param name="score">Score counted.</param>
        public CountHandCommand(int playerId, int score)
            : base(playerId)
        {
            Score = score;
        }

        /// <summary>
        /// Gets the score that was counted.
        /// </summary>
        public int Score { get; }
    }
}
