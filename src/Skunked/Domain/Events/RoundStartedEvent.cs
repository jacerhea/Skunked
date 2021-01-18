using System;

namespace Skunked.Domain.Events
{
    public class RoundStartedEvent : StreamEvent
    {
        public RoundStartedEvent(Guid gameId, int version) : base(gameId, version)
        {
        }
    }
}
