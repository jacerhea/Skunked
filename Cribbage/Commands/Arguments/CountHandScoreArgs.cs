using System;
using Skunked.Score.Interface;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public class CountHandScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountHandScoreArgs(GameState gameState, int playerId, int round, IScoreCalculator scoreCalculator, int playerCountedScore)
            : base(gameState, playerId, round)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (playerCountedScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator;
            PlayerCountedScore = playerCountedScore;
        }
    }
}
