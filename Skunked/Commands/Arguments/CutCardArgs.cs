using System;
using Skunked.PlayingCards;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public class CutCardArgs : CommandArgsBase
    {
        public Card CutCard { get; private set; }
        public IOrderStrategy OrderStrategy { get; private set; }

        public CutCardArgs(GameEventStream eventStream, GameState gameState, int playerId, int round, Card cutCard, IOrderStrategy orderStrategy = null)
            : base(gameState, playerId, round)
        {
            if (cutCard == null) throw new ArgumentNullException("cutCard");
            CutCard = cutCard;
            OrderStrategy = orderStrategy ?? new StandardOrder();
        }
    }
}
