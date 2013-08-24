using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Games.Domain.MainModule.Entities.CardGames.Exceptions
{
    public class InvalidGamePlayOperation : InvalidOperationException
    {
        public InvalidGamePlayOperation(string message)
            : base(message)
        {

        }
    }
}
