using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Games.Domain.MainModule.Entities.PlayingCards;
using Games.Infrastructure.CrossCutting;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
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
        public SerializableCard StartingCard { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int Round { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializableKeyValuePair<int, List<SerializableCard>>> PlayerDealtCards { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<SerializableKeyValuePair<int, List<SerializableCard>>> PlayerHand { get; set; }

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
        public List<SerializableCard> Crib { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public List<PlayerScoreShow> PlayerShowScores { get; set; }
    }
}
