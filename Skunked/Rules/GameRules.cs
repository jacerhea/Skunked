using System;

namespace Skunked.Rules
{
    public class GameRules
    {
        public GameScoreType ScoreType { get; set; }
        /// <summary>
        /// 31
        /// </summary>
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

        public GameRules(GameScoreType scoreType = GameScoreType.Standard121, int numberOfPlayers = 2)
        {
            if (numberOfPlayers != 2 && numberOfPlayers != 4) { throw new ArgumentOutOfRangeException(); }
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }
    }
}