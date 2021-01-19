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
        /// <param name="playerId"></param>
        /// <param name="hand"></param>
        public PlayerHand(int playerId, List<Card> hand)
        {
            PlayerId = playerId;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }

        public int PlayerId { get; }

        public List<Card> Hand { get; }
    }
}
