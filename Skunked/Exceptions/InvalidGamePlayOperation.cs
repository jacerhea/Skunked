using System;

namespace Skunked.Exceptions
{
    [Serializable]
    public abstract class InvalidGamePlayOperation : InvalidOperationException
    {
        protected InvalidGamePlayOperation(string message)
            : base(message)
        {

        }
    }
}
