using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;

namespace Skunked.Players
{
    public class PlayerHand
    {
        public Player Player { get; private set; }
        public List<Card> Hand { get; private set; }

        public PlayerHand(Player player, List<Card> hand)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            if (hand == null) throw new ArgumentNullException(nameof(hand));
            Player = player;
            Hand = hand;
        }

        public override string ToString()
        {
            return $"{Player}: {{{string.Join(", ", (Hand.Select(c => c.ToString()).ToArray()))}}}";
        }
    }
}