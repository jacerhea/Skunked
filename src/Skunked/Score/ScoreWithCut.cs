using Skunked.Cards;

namespace Skunked.Score
{

    /// <summary>
    /// Represents a starter card and a score.
    /// </summary>
    public class ScoreWithCut
    {
        public int Score { get; set; }

        public Card Cut { get; set; }
    }
}