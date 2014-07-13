namespace Skunked.State
{
    public class PlayerScoreShow
    {
        public int Player { get; set; }
        public int PlayerCountedShowScore { get; set; }
        public int ShowScore { get; set; }
        public bool HasShowed { get; set; }
        public bool HasShowedCrib { get; set; }
        public bool Complete { get; set; }
        public int? CribScore { get; set; }
    }
}
