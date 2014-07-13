using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Utility;

namespace Skunked.State
{
    [DebuggerDisplay("Round {Round} - Done: {Complete}")]
    public class RoundState
    {
        public Card Starter { get; set; }
        public int Round { get; set; }
        public List<CustomKeyValuePair<int, List<Card>>> DealtCards { get; set; }
        public List<CustomKeyValuePair<int, List<Card>>> Hands { get; set; }
        public List<List<PlayerPlayItem>> PlayedCards { get; set; }
        public bool ThrowCardsComplete { get; set; }
        public bool PlayedCardsComplete { get; set; }
        public bool Complete { get; set; }
        public int PlayerCrib { get; set; }
        public List<Card> Crib { get; set; }
        public List<PlayerScoreShow> ShowScores { get; set; }
    }
}
