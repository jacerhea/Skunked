using System;
using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// Event when the game has started.
    /// </summary>
    public class GameStartedEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameStartedEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="players"></param>
        /// <param name="rules"></param>
        public GameStartedEvent(Guid gameId, int version, List<int> players, GameRules rules)
            : base(gameId, version)
        {
            Players = players;
            Rules = rules;
        }

        public List<int> Players { get; }

        public GameRules Rules { get; }
    }
}
