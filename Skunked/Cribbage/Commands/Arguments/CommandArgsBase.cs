using System;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public abstract class CommandArgsBase
    {
        public GameState GameState { get; private set; }
        public int PlayerID { get; private set;}
        public int Round { get; private set; }

        protected CommandArgsBase(GameState gameState, int playerID, int round)
        {
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (playerID < 0) throw new ArgumentOutOfRangeException("playerID");
            if (round < 0) throw new ArgumentOutOfRangeException("round");
            GameState = gameState;
            PlayerID = playerID;
            Round = round;
        }
    }
}
