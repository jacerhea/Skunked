using Skunked.Cards;

namespace Skunked.Domain.State
{
    public class PlayItem
    {
        public int Player { get; set; }
        public Card Card { get; set; }
        public int Score { get; set; }
        public int? NextPlayer { get; set; }
        public int NewCount { get; set; }
    }
}
