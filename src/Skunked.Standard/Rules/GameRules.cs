using System;

namespace Skunked.Rules
{
    /// <summary>
    /// Set of Cribbage rules.
    /// </summary>
    public class GameRules
    {
        public WinningScoreType ScoreType { get; set; }
        public int HandSizeToDeal => PlayerCount == 2 ? 6 : 5;
        public int PlayerCount { get; set; }
        public int WinningScore => ScoreType == WinningScoreType.Standard121 ? 121 : 61;
        /// <summary>
        /// 
        /// </summary>
        public static int HandSize => 4;


        public GameRules() : this(WinningScoreType.Standard121, 2)
        {
        }

        public GameRules(WinningScoreType scoreType, int numberOfPlayers)
        {
            if (numberOfPlayers != 2 && numberOfPlayers != 4) { throw new ArgumentOutOfRangeException(nameof(numberOfPlayers)); }
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Scores
        {
            /// <summary>
            /// 
            /// </summary>
            public static int Go => 1;

            /// <summary>
            /// 
            /// </summary>
            public static int PlayMaxScore => 31;
            /// <summary>
            /// 
            /// </summary>
            public static int FifteenScore => 2;
            /// <summary>
            /// 
            /// </summary>
            public static int NibsScore => 2;
            /// <summary>
            /// 
            /// </summary>
            public static int NobsScore => 1;
            /// <summary>
            /// 
            /// </summary>
            public static int PairScore => 2;
            /// <summary>
            /// 
            /// </summary>
            public static int PairRoyalScore => 6;
            /// <summary>
            /// 
            /// </summary>
            public static int DoublePairRoyalScore => 12;
            /// <summary>
            /// 
            /// </summary>
            public static int FourCardFlush => 4;
            /// <summary>
            /// 
            /// </summary>
            public static int FiveCardFlush => 5;
        }
    }
}