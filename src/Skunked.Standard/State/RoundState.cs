using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;

namespace Skunked.State
{
    public class RoundState
    {
        public Card Starter { get; set; }
        public int Round { get; set; }
        public List<PlayerHand> DealtCards { get; set; } = new List<PlayerHand>();
        public List<PlayerHand> Hands { get; set; } = new List<PlayerHand>();
        public List<List<PlayItem>> ThePlay { get; set; } = new List<List<PlayItem>>();
        public bool ThrowCardsComplete { get; set; }
        public bool PlayedCardsComplete { get; set; }
        public bool Complete { get; set; }
        public int PlayerCrib { get; set; }
        public List<Card> Crib { get; set; } = new List<Card>();
        public List<PlayerScoreShow> ShowScores { get; set; } = new List<PlayerScoreShow>();
        public PreRound PreRound { get; set; }
    }

    public class PreRound
    {
        public List<Card> Deck { get; set; } = new List<Card>();
    }
}
