using System;
using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.Players
{
    public class PlayerHand
    {
        public int PlayerId { get; }
        public List<Card> Hand { get; }

        public PlayerHand(int playerId, List<Card> hand)
        {
            PlayerId = playerId;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }
    }
}