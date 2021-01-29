namespace Skunked.Domain.Commands
{
    /// <summary>
    /// Base class for all commands.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        /// <param name="playerId">The id of player.</param>
        public CommandBase(int playerId)
        {
            PlayerId = playerId;
        }

        /// <summary>
        /// Gets the id of the player associated with the command.
        /// </summary>
        public int PlayerId { get; }
    }
}