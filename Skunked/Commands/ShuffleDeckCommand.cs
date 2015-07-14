using System;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class ShuffleDeckCommand : CribbageCommandBase, ICommand
    {
        private readonly ShuffleDeckCommandArgs _args;

        public ShuffleDeckCommand(ShuffleDeckCommandArgs args)
            : base(args)
        {
            _args = args;
        }

        protected override void ValidateState()
        {
            ValidateStateBase();
        }

        public void Execute()
        {


        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
