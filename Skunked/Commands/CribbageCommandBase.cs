using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Skunked.Exceptions;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Commands
{
    public abstract class CribbageCommandBase
    {
        private readonly CommandArgsBase _args;

        protected CribbageCommandBase(CommandArgsBase args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            _args = args;
        }

        protected GameState GameState { get { return _args.GameState; } }

        protected void ValidateStateBase()
        {
            CheckEndOfGame();
            if (_args.GameState.PlayerIds.All(id => id == _args.PlayerId)) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidPlayer); }

            if (_args.GameState.Rounds.Count(r => r.Round == _args.Round) != 1)
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
            var gameState = _args.GameState;
            if (gameState.IsGameFinished())
            {
                if (gameState.CompletedAt == null)
                {
                    gameState.CompletedAt = DateTimeOffset.Now;
                    gameState.GetCurrentRound().Complete = true;
                }
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.GameFinished);
            }
        }

        protected void UndoBase()
        {
            var stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, _args.GameState);
            stream.Seek(0, SeekOrigin.Begin);
            var gameState = (GameState)formatter.Deserialize(stream);

        }

        protected abstract void ValidateState();
    }
}
