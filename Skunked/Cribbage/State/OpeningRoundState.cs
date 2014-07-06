using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Utility;

namespace Skunked.State
{
    [DebuggerDisplay("Is Complete: {IsDone}")]
    public class OpeningRoundState
    {
        public List<Card> Deck { get; set; }
        public List<CustomKeyValuePair<int, Card>> PlayersCutCard { get; set; }
        public bool IsDone { get; set; }
        public int? WinningPlayerCut { get; set; }
    }
}
