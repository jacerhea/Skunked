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
        public int PlayerId { get; }
        public List<Card> Hand { get; }

        /// <summary>
        /// Initializes a new instance of PlayerHand with the player's id and their hand.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="hand"></param>
        public PlayerHand(int playerId, List<Card> hand)
        {
            PlayerId = playerId;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }
    }
}