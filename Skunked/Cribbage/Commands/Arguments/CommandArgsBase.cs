using System;
using Cribbage.State;

namespace Cribbage.Commands.Arguments
{
    public abstract class CommandArgsBase
    {
        public CribGameState GameState { get; private set; }
        public int PlayerID { get; private set;}
        public int Round { get; private set; }

        public CommandArgsBase(CribGameState cribGameState, int playerID, int round)
        {
            if (cribGameState == null) throw new ArgumentNullException("cribGameState");
            if (playerID < 0) throw new ArgumentOutOfRangeException("playerID");
            if (round < 0) throw new ArgumentOutOfRangeException("round");
            GameState = cribGameState;
            PlayerID = playerID;
            Round = round;
        }
    }
}
