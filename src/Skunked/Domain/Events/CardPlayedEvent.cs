using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// A Card has been played.
    /// </summary>
    public class CardPlayedEvent : StreamEvent
    {
        /// <summary>
        /// The player id. 
        /// </summary>
        public int PlayerId { get; set; }
        
        /// <summary>
        /// The Card being played.
        /// </summary>
        public Card Played { get; set; }
    }
}