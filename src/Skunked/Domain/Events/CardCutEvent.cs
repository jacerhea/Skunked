using Skunked.Cards;

namespace Skunked.Domain.Events
{
    /// <summary>
    /// A card has been cut.
    /// </summary>
    public class CardCutEvent : StreamEvent
    {
        /// <summary>
        /// The player's id who cut the card.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// The card that was cut.
        /// </summary>
        public Card CutCard { get; set; }
    }
}