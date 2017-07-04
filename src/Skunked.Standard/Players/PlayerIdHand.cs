using System;
using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.Players
{
    public class PlayerIdHand
    {
        public int Id { get; }
        public List<Card> Hand { get; }

        public PlayerIdHand(int id, List<Card> hand)
        {
            Id = id;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }
    }
}