using System;
using System.Runtime.Serialization;

namespace Cribbage
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class SerializableCard : Card, IEquatable<SerializableCard>
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public Rank Rank { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public Suit Suit { get; set; }

        public SerializableCard()
        {
            Rank = Rank.Ace;
            Suit = Suit.Clubs;
        }

        public SerializableCard(Card card)
        {
            Rank = card.Rank;
            Suit = card.Suit;
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", Rank, Suit);
        }

        public bool Equals(Card other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Rank == Rank && other.Suit == Suit;
        }

        public bool Equals(SerializableCard other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Rank == Rank && other.Suit == Suit;
        }

        public override int GetHashCode()
        {
            return ((int)this.Rank) ^ ((int)this.Suit);
        }
    }
}
