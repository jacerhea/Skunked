using System.Runtime.Serialization;

namespace Cribbage.Rules
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public class CribGameRules
    {
        [DataMember]
        public GameScoreType ScoreType { get; set; }
        [DataMember]
        public int PlayMaxScore { get { return 31; } }
        [DataMember]
        public int HandSize { get { return 4; } }
        [DataMember]
        public int HandSizeToDeal { get { return PlayerCount == 2 ? 6 : 5; } }
        [DataMember]
        public int PlayerCount { get; set; }
        [DataMember]
        public int WinningScore { get { return ScoreType == GameScoreType.Standard121 ? 121 : 61; } }

        public CribGameRules()
        {
            PlayerCount = 2;
            ScoreType = GameScoreType.Standard121;
        }

        public CribGameRules(GameScoreType scoreType, int numberOfPlayers)
        {
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }
    }
}