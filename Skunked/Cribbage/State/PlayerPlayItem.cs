using System.Runtime.Serialization;

namespace Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class PlayerPlayItem
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public int Player { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public SerializableCard Card { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Score { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int? NextPlayer { get; set; }
    }
}
