using System.Collections.Generic;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.State
{
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
        public PreRound PreRound { get; set; }
    }

    public class PreRound
    {
        public List<Card> Deck { get; set; }
    }
}
