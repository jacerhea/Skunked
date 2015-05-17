using System;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public abstract class CommandArgsBase
    {
        public GameEventStream EventStream { get; private set; }
        public GameState GameState { get; private set; }
        public int PlayerId { get; private set;}
        public int Round { get; private set; }

        protected CommandArgsBase(GameEventStream eventStream, GameState gameState, int playerId, int round)
        {
            if (eventStream == null) throw new ArgumentNullException("eventStream");
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (playerId < 0) throw new ArgumentOutOfRangeException("playerId");
            if (round < 0) throw new ArgumentOutOfRangeException("round");
            EventStream = eventStream;
            GameState = gameState;
            PlayerId = playerId;
            Round = round;
        }
    }
}
