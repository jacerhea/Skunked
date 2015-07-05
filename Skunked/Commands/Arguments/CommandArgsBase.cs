using System;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public abstract class CommandArgsBase
    {
        public EventStream EventStream { get; private set; }
        public GameState GameState { get; private set; }
        public int PlayerId { get; private set;}
        public int Round { get; private set; }

        protected CommandArgsBase(GameState gameState, int playerId, int round)
        {
            if (gameState == null) throw new ArgumentNullException(nameof(gameState));
            if (playerId < 0) throw new ArgumentOutOfRangeException(nameof(playerId));
            if (round < 0) throw new ArgumentOutOfRangeException(nameof(round));
            GameState = gameState;
            PlayerId = playerId;
            Round = round;
        }
    }
}
