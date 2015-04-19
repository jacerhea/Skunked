using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;

namespace Skunked.Players
{
    public class PlayerIdHand
    {
        public int Id { get; private set; }
        public List<Card> Hand { get; private set; }

        public PlayerIdHand(int id, List<Card> hand)
        {
            if (hand == null) throw new ArgumentNullException("hand");
            Id = id;
            Hand = hand;
        }

        public override string ToString()
        {
            return string.Format("{0}: {{{1}}}", Id, string.Join(", ", (Hand.Select(c => c.ToString()).ToArray())));
        }
    }
}