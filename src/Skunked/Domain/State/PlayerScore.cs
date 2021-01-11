namespace Skunked.Domain.State
{
    /// <summary>
    /// Player Id and a score.
    /// </summary>
    public class PlayerScore
    {
        /// <summary>
        /// Player Id
        /// </summary>
        public int Player { get; set; }
        /// <summary>
        /// The player's score.
        /// </summary>
        public int Score { get; set; }
    }
}
