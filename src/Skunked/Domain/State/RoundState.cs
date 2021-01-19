using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;

namespace Skunked.Domain.State
{
    public class RoundState
    {
        public Card Starter { get; set; }

        public int Round { get; set; }

        public List<PlayerHand> DealtCards { get; set; } = new ();

        public List<PlayerHand> Hands { get; set; } = new ();

        public List<List<PlayItem>> ThePlay { get; set; } = new ();

        public bool ThrowCardsComplete { get; set; }

        public bool PlayedCardsComplete { get; set; }

        public bool Complete { get; set; }

        public int PlayerCrib { get; set; }

        public List<Card> Crib { get; set; } = new ();

        public List<PlayerScoreShow> ShowScores { get; set; } = new ();

        public PreRound PreRound { get; set; }
    }
}
