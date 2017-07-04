using System.Collections.Generic;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.State
{
    public class OpeningRound
    {
        public List<Card> Deck { get; set; }
        public List<PlayerIdCard> CutCards { get; set; }
        public bool Complete { get; set; }
        public int? WinningPlayerCut { get; set; }
    }
}
