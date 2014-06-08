using System;

namespace Skunked.Exceptions
{
    public class InvalidGamePlayOperation : InvalidOperationException
    {
        public InvalidGamePlayOperation(string message)
            : base(message)
        {

        }
    }
}
