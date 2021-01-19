using System;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    ///
    /// </summary>
    public class StarterCardSelectedEvent : StreamEvent
    {
        public StarterCardSelectedEvent(Guid gameId, int version, Card starter) : base(gameId, version)
        {
            Starter = starter;
        }

        public Card Starter { get; }
    }
}
