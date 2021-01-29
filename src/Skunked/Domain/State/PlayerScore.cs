namespace Skunked.Domain.State
{
    /// <summary>
    /// Player Id and a score.
    /// </summary>
    public class PlayerScore
    {
        /// <summary>
        /// Gets or sets player Id.
        /// </summary>
        public int Player { get; set; }

        /// <summary>
        /// Gets or sets the player's score.
        /// </summary>
        public int Score { get; set; }
    }
}
