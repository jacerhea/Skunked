using System;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;
using Games.Domain.MainModule.Entities.PlayingCards;
using Games.Domain.MainModule.Entities.PlayingCards.Order;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments
{
    public class CutCardArgs : CommandArgsBase
    {
        public ICard CutCard { get; set; }
        public IOrderStrategy OrderStrategy { get; set; }

        public CutCardArgs(CribGameState gameState, int playerID, int round, ICard cutCard, IOrderStrategy orderStrategy) : base(gameState, playerID, round)
        {
            if (cutCard == null) throw new ArgumentNullException("cutCard");
            if (orderStrategy == null) throw new ArgumentNullException("orderStrategy");
            CutCard = cutCard;
            OrderStrategy = orderStrategy;
        }
    }
}
