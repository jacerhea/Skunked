using System;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Player
{
    public class PlayerCard
    {
        public ICribPlayer Player { get; private set; }
        public ICard Card { get; private set; }

        public PlayerCard(ICribPlayer player, ICard card)
        {
            if (player == null) throw new ArgumentNullException("player");
            if (card == null) throw new ArgumentNullException("card");
            Player = player;
            Card = card;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Player.Name, Card.ToString());
        }
    }
}
