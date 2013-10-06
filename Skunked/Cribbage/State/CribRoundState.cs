using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Cribbage.Utility;

namespace Cribbage.State
{
#if !SILVERLIGHT
    [Serializable]
#endif
    [DebuggerDisplay("Round {0} - Done: {1}")]
    public class CribRoundState
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public Card StartingCard { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Round { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializableKeyValuePair<int, List<Card>>> PlayerDealtCards { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializableKeyValuePair<int, List<Card>>> PlayerHand { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<List<PlayerPlayItem>> PlayersShowedCards { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool ThrowCardsIsDone { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool PlayCardsIsDone { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool IsDone { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int PlayerCrib { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<Card> Crib { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<PlayerScoreShow> PlayerShowScores { get; set; }
    }
}
