using System.Collections.Generic;
using System.Diagnostics;

namespace Skunked.State
{
    [DebuggerDisplay("Score: {Score}")]
    public class TeamScore
    {
        public List<int> Players { get; set; }
        public int Score { get; set; }
    }
}
