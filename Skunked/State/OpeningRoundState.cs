using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Utility;

namespace Skunked.State
{
    [DebuggerDisplay("Complete = {Complete}")]
    public class OpeningRoundState
    {
        public List<Card> Deck { get; set; }
        public List<CustomKeyValuePair<int, Card>> CutCards { get; set; }
        public bool Complete { get; set; }
        public int? WinningPlayerCut { get; set; }
    }
}
