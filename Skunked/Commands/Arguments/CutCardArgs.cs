using System;
using Skunked.PlayingCards.Order;
using Skunked.PlayingCards.Order.Interface;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public class CutCardArgs : CommandArgsBase
    {
        public Card CutCard { get; private set; }
        public IOrderStrategy OrderStrategy { get; private set; }

        public CutCardArgs(GameState gameState, int playerId, int round, Card cutCard, IOrderStrategy orderStrategy = null) : base(gameState, playerId, round)
        {
            if (cutCard == null) throw new ArgumentNullException("cutCard");
            CutCard = cutCard;
            OrderStrategy = orderStrategy ?? new StandardOrder();
        }
    }
}
