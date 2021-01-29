using System;
using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Players
{
    /// <summary>
    /// A player id and their hand.
    /// </summary>
    public class PlayerHand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHand"/> class.
        /// </summary>
        /// <param name="playerId">The id of the player.</param>
        /// <param name="hand"></param>
        public PlayerHand(int playerId, List<Card> hand)
        {
            PlayerId = playerId;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }

        /// <summary>
        /// Gets the id of the player.
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the hand of the player.
        /// </summary>
        public List<Card> Hand { get; }
    }
}
