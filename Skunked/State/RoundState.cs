using System.Collections.Generic;
using System.Diagnostics;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.State
{
    [DebuggerDisplay("Round: {Round} - Complete: {Complete}")]
    public class RoundState
    {
        public Card Starter { get; set; }
        public int Round { get; set; }
        public List<PlayerIdHand> DealtCards { get; set; }
        public List<PlayerIdHand> Hands { get; set; }
        public List<List<PlayerPlayItem>> ThePlay { get; set; }
        public bool ThrowCardsComplete { get; set; }
        public bool PlayedCardsComplete { get; set; }
        public bool Complete { get; set; }
        public int PlayerCrib { get; set; }
        public List<Card> Crib { get; set; }
        public List<PlayerScoreShow> ShowScores { get; set; }
    }
}
