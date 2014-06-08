using System;
using Skunked;
using Skunked.PlayingCards.Order.Interface;
using Skunked.State;

namespace Cribbage.Commands.Arguments
{
    public class CutCardArgs : CommandArgsBase
    {
        public Card CutCard { get; set; }
        public IOrderStrategy OrderStrategy { get; set; }

        public CutCardArgs(GameState gameState, int playerID, int round, Card cutCard, IOrderStrategy orderStrategy) : base(gameState, playerID, round)
        {
            if (cutCard == null) throw new ArgumentNullException("cutCard");
            if (orderStrategy == null) throw new ArgumentNullException("orderStrategy");
            CutCard = cutCard;
            OrderStrategy = orderStrategy;
        }
    }
}
