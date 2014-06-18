using System;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CutCardCommand : CribbageCommandBase, ICommand
    {
        private readonly CutCardArgs _args;

        public CutCardCommand(CutCardArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateState();
            if (_args.GameState.OpeningRoundState.PlayersCutCard.All(kv => kv.Key != _args.PlayerId))
            {
                _args.GameState.OpeningRoundState.PlayersCutCard.Add(new CustomKeyValuePair<int, Card> { Key = _args.PlayerId, Value = new Card(_args.CutCard) });
            }

            bool isDone = (_args.GameState.GameRules.PlayerCount == _args.GameState.OpeningRoundState.PlayersCutCard.Count);
            _args.GameState.OpeningRoundState.IsDone = isDone;

            if (isDone && _args.GameState.Rounds.Count == 0)
            {
                var winningPlayerCut = _args.GameState.OpeningRoundState.PlayersCutCard.MinBy(playerCard => _args.OrderStrategy.Order(playerCard.Value));
                _args.GameState.OpeningRoundState.WinningPlayerCut = winningPlayerCut.Key;

                var command = new CreateNewRoundCommand(_args.GameState, 0);
                command.Execute();
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void ValidateState()
        {
            if(_args.GameState.OpeningRoundState.PlayersCutCard.Any(kv => kv.Key == _args.PlayerId))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);    
            }

            if (_args.GameState.OpeningRoundState.PlayersCutCard.Any(kv => kv.Value.Equals(_args.CutCard)))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }
        }
    }
}
