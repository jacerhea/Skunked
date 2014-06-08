using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Players
{
    public class PlayerHand
    {
        public Player Player { get; private set; }
        public List<Card> Hand { get; private set; }

        public PlayerHand(Player player, List<Card> playersHand)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (playersHand == null) throw new ArgumentNullException("playersHand");
            Player = player;
            Hand = playersHand;
        }

        public override string ToString()
        {
            return string.Format("{0}: {{{1}}}", Player, string.Join(", ", (Hand.Select(c => c.ToString()).ToArray())));
        }
    }
}