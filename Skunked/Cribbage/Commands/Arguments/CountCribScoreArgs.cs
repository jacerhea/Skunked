using System;
using Cribbage.Score.Interface;
using Cribbage.State;

namespace Cribbage.Commands.Arguments
{
    public class CountCribScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountCribScoreArgs(CribGameState cribGameState, int playerID, int round, IScoreCalculator scoreCalculator, int playerCountedCribScore)
            : base(cribGameState, playerID, round)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (playerCountedCribScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator;
            PlayerCountedScore = playerCountedCribScore;
        }
    }
}
