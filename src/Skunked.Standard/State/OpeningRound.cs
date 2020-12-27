using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;

namespace Skunked.State
{
    public class OpeningRound
    {
        public List<Card> Deck { get; set; } = new();
        public List<PlayerIdCard> CutCards { get; set; }
        public bool Complete { get; set; }
        public int? WinningPlayerCut { get; set; }
    }
}
