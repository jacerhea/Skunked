using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Skunked.Utility;

namespace Skunked.State
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
        public List<CustomKeyValuePair<int, Card>> PlayersCutCard { get; set; }
    
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
