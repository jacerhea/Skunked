using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Utility;

namespace Skunked.State
{
    [DebuggerDisplay("Round {0} - Done: {1}")]
    public class RoundState
    {
        public Card StartingCard { get; set; }
        public int Round { get; set; }
        public List<CustomKeyValuePair<int, List<Card>>> PlayerDealtCards { get; set; }
        public List<CustomKeyValuePair<int, List<Card>>> PlayerHand { get; set; }
        public List<List<PlayerPlayItem>> PlayersShowedCards { get; set; }
        public bool ThrowCardsIsDone { get; set; }
        public bool PlayCardsIsDone { get; set; }
        public bool IsDone { get; set; }
        public int PlayerCrib { get; set; }
        public List<Card> Crib { get; set; }
        public List<PlayerScoreShow> PlayerShowScores { get; set; }
    }
}
