using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.State
{
    [DebuggerDisplay("Complete = {Complete}")]
    public class OpeningRoundState
    {
        public List<Card> Deck { get; set; }
        public List<PlayerIdCard> CutCards { get; set; }
        public bool Complete { get; set; }
        public int? WinningPlayerCut { get; set; }
    }
}
