using System;

namespace Skunked.Exceptions
{
    [Serializable]
    public class InvalidGamePlayOperation : InvalidOperationException
    {
        public InvalidGamePlayOperation(string message)
            : base(message)
        {

        }
    }
}
