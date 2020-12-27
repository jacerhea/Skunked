using System.Collections.Generic;

namespace Skunked.State
{
    public class TeamScore
    {
        public List<int> Players { get; set; } = new();
        public int Score { get; set; }
    }
}
