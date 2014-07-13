using System.Diagnostics;

namespace Skunked.State
{
    [DebuggerDisplay("Player: {Player}, Score: {Score}")]
    public class PlayerScore
    {
        public int Player { get; set; }
        public int Score { get; set; }
    }
}
