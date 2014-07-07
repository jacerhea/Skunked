using System;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Commands
{
    public abstract class CribbageCommandBase
    {
        private readonly CommandArgsBase _args;

        protected CribbageCommandBase(CommandArgsBase args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        protected void ValidateStateBase()
        {
            CheckEndOfGame();
            if (_args.GameState.Players.All(sp => sp.Id != _args.PlayerId)) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidPlayer); }

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
