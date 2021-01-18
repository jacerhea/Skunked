using System;

namespace Skunked.Domain.Events
{
    public class GameCompletedEvent : StreamEvent
    {
        public GameCompletedEvent(Guid gameId, int version) : base(gameId, version)
        {
        }
    }
}
