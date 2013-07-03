using System;
using System.Linq;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands
{
    public abstract class CribbageCommandBase
    {
        private readonly CommandArgsBase _args;

        public CribbageCommandBase(CommandArgsBase args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        protected void ValidateStateBase()
        {
            CheckEndOfGame();
            if (_args.GameState.Players.All(sp => sp.ID != _args.PlayerID)) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidPlayer); }

            if(_args.GameState.Rounds.Count(r => r.Round == _args.Round) != 1)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidRequest);
            }
            
            ValidateState();
        }

        protected void EndofCommandCheck()
        {
            CheckEndOfGame();
        }

        private void CheckEndOfGame()
        {
            if (_args.GameState.IsGameFinished())
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.GameFinished);
            }            
        }

        protected abstract void ValidateState();
    }
}
