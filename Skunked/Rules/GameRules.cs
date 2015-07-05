using System;
using System.Runtime.Serialization;

namespace Skunked.Rules
{
    public class GameRules : ISerializable
    {
        public GameScoreType ScoreType { get; set; }
        public int HandSizeToDeal { get { return PlayerCount == 2 ? 6 : 5; } }
        public int PlayerCount { get; set; }
        public int WinningScore { get { return ScoreType == GameScoreType.Standard121 ? 121 : 61; } }

        public static int GoSore { get { return 1; } }

        public static int HandSize { get { return 4; } }
        public static int PlayMaxScore { get { return 31; } }
        public static int FifteenScore { get { return 2; } }
        public static int NibsScore { get { return 2; } }
        public static int NobsScore { get { return 1; } }
        public static int PairScore { get { return 2; } }
        public static int PairRoyalScore { get { return 6; } }
        public static int DoublePairRoyalScore { get { return 12; } }
        public static int FourCardFlush { get { return 4; } }
        public static int FiveCardFlush { get { return 5; } }

        public GameRules() : this(GameScoreType.Standard121, 2)
        {
        }

        public GameRules(GameScoreType scoreType, int numberOfPlayers)
        {
            if (numberOfPlayers != 2 && numberOfPlayers != 4) { throw new ArgumentOutOfRangeException(); }
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ScoreType", ScoreType);
            info.AddValue("PlayerCount", PlayerCount);
        }
    }
}