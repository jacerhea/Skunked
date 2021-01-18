using System;
using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class CardsThrownEvent : StreamEvent
    {

        public CardsThrownEvent(Guid gameId, int version, int playerId, List<Card> thrown) : base(gameId, version)
        {
            PlayerId = playerId;
            Thrown = thrown;
        }

        /// <summary>
        /// The player id. 
        /// </summary>
        public int PlayerId { get; }
        public List<Card> Thrown { get; }
    }
}