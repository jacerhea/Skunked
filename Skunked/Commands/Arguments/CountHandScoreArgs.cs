using System;
using Skunked.Score;
using Skunked.State;

namespace Skunked.Commands
{
    public class CountHandScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountHandScoreArgs(GameState gameState, int playerId, int round, int playerCountedScore, IScoreCalculator scoreCalculator = null)
            : base(gameState, playerId, round)
        {
            if (playerCountedScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
            PlayerCountedScore = playerCountedScore;
        }
    }
}
