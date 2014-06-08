using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Cribbage.Utility;
using Skunked;

namespace Cribbage.State
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
        public List<Card> Deck { get; set; }

        #if !SILVERLIGHT
        [DataMember]
        #endif
        public List<SerializableKeyValuePair<int, Card>> PlayersCutCard { get; set; }
    
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
