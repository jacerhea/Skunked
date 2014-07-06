using System;

namespace Skunked.Players
{
    public class PlayerCard
    {
        public Player Player { get; private set; }
        public Card Card { get; private set; }

        public PlayerCard(Player player, Card card)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (card == null) throw new ArgumentNullException("card");
            Player = player;
            Card = card;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Player.Name, Card);
        }
    }
}
