using System.Runtime.Serialization;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.State
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class PlayerScoreShow
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public int Player { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int PlayerCountedShowScore { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int ShowScore { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool HasShowed { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool HasShowedCrib { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public bool IsDone { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public int? CribScore { get; set; }
    }
}
