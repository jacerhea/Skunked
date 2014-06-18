using System;
using Skunked.Score.Interface;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public class CountCribScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountCribScoreArgs(GameState gameState, int playerId, int round, IScoreCalculator scoreCalculator, int playerCountedCribScore)
            : base(gameState, playerId, round)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (playerCountedCribScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator;
            PlayerCountedScore = playerCountedCribScore;
        }
    }
}
