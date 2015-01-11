using System;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public abstract class CommandArgsBase
    {
        public GameState GameState { get; private set; }
        public int PlayerId { get; private set;}
        public int Round { get; private set; }

        protected CommandArgsBase(GameState gameState, int playerId, int round)
        {
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (playerId < 0) throw new ArgumentOutOfRangeException("playerId");
            if (round < 0) throw new ArgumentOutOfRangeException("round");
            GameState = gameState;
            PlayerId = playerId;
            Round = round;
        }
    }
}
