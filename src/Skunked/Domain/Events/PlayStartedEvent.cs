using System;

namespace Skunked.Domain.Events
{
    public class PlayStartedEvent : StreamEvent
    {
        public PlayStartedEvent(Guid gameId, int version, int round)
            : base(gameId, version)
        {
            Round = round;
        }

        public int Round { get; }
    }
}
