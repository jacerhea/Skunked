using System;
using Skunked.Score;
using Skunked.Score.Interface;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public class CountCribScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountCribScoreArgs(GameState gameState, int playerId, int round, int playerCountedCribScore, IScoreCalculator scoreCalculator = null)
            : base(gameState, playerId, round)
        {
            if (playerCountedCribScore < 0) throw new ArgumentOutOfRangeException("playerCountedCribScore");
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
            PlayerCountedScore = playerCountedCribScore;
        }
    }
}
