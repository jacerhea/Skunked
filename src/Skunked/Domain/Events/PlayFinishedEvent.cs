using System;

namespace Skunked.Domain.Events
{
    public class PlayFinishedEvent : StreamEvent
    {
        public PlayFinishedEvent(Guid gameId, int version, int round)
            : base(gameId, version)
        {
            Round = round;
        }

        public int Round { get; }
    }
}
