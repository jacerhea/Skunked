using System;

namespace Skunked.Rules
{
    public class GameRules
    {
        public GameScoreType ScoreType { get; set; }
        public int HandSizeToDeal => PlayerCount == 2 ? 6 : 5;
        public int PlayerCount { get; set; }
        public int WinningScore => ScoreType == GameScoreType.Standard121 ? 121 : 61;

        public static int GoSore => 1;

        public static int HandSize => 4;
        public static int PlayMaxScore => 31;
        public static int FifteenScore => 2;
        public static int NibsScore => 2;
        public static int NobsScore => 1;
        public static int PairScore => 2;
        public static int PairRoyalScore => 6;
        public static int DoublePairRoyalScore => 12;
        public static int FourCardFlush => 4;
        public static int FiveCardFlush => 5;

        public GameRules() : this(GameScoreType.Standard121, 2)
        {
        }

        public GameRules(GameScoreType scoreType, int numberOfPlayers)
        {
            if (numberOfPlayers != 2 && numberOfPlayers != 4) { throw new ArgumentOutOfRangeException(); }
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }
    }
}