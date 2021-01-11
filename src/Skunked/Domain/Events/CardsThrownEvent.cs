using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class CardsThrownEvent : StreamEvent
    {
        /// <summary>
        /// The player id. 
        /// </summary>
        public int PlayerId { get; set; }
        public List<Card> Thrown { get; set; } = new();
    }
}