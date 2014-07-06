﻿namespace Skunked.Rules
{
    public class GameRules
    {
        public GameScoreType ScoreType { get; set; }
        public int PlayMaxScore { get { return 31; } }
        public int HandSize { get { return 4; } }
        public int HandSizeToDeal { get { return PlayerCount == 2 ? 6 : 5; } }
        public int PlayerCount { get; set; }
        public int WinningScore { get { return ScoreType == GameScoreType.Standard121 ? 121 : 61; } }

        public GameRules(GameScoreType scoreType = GameScoreType.Standard121, int numberOfPlayers = 2)
        {
            PlayerCount = numberOfPlayers;
            ScoreType = scoreType;
        }
    }
}