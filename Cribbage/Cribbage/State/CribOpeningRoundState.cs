using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Games.Domain.MainModule.Entities.PlayingCards;
using Games.Infrastructure.CrossCutting;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    [DebuggerDisplay("Complete {IsDone}")]
    public class CribOpeningRoundState
    {
        #if !SILVERLIGHT
        [DataMember]
        #endif
        public List<SerializableCard> Deck { get; set; }

        #if !SILVERLIGHT
        [DataMember]
        #endif
        public List<SerializableKeyValuePair<int, SerializableCard>> PlayersCutCard { get; set; }
    
        #if !SILVERLIGHT
        [DataMember]
        #endif
        public bool IsDone { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int? WinningPlayerCut { get; set; }
    }
}
