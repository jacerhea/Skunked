using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Skunked.PlayingCards;

namespace Skunked.Players
{
    public class PlayerIdHand : ISerializable
    {
        public int Id { get; set; }
        public List<Card> Hand { get; set; }

        /// <summary>
        /// Just used for serialization
        /// </summary>
        public PlayerIdHand()
        {
            
        }

        public PlayerIdHand(int id, List<Card> hand)
        {
            Id = id;
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Hand", Hand);
        }
    }
}