using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    [DebuggerDisplay("Player: {Player}, Score: {Score}")]
    public class SerializablePlayerScore
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public int Player { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Score { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int WinningScore { get; set; }
    }
}
