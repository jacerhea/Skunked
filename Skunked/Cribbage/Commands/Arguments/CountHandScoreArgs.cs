using System;
using Skunked.Score.Interface;
using Skunked.State;

namespace Cribbage.Commands.Arguments
{
    public class CountHandScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountHandScoreArgs(GameState gameState, int playerID, int round, IScoreCalculator scoreCalculator, int playerCountedScore)
            : base(gameState, playerID, round)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (playerCountedScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator;
            PlayerCountedScore = playerCountedScore;
        }
    }
}
