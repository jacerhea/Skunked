namespace Skunked.State
{
    public class PlayerPlayItem
    {
        public int Player { get; set; }
        public Card Card { get; set; }
        public int Score { get; set; }
        public int? NextPlayer { get; set; }
    }
}
