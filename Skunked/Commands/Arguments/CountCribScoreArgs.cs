using System;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public class CountCribScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountCribScoreArgs(GameEventStream eventStream, GameState gameState, int playerId, int round, int playerCountedCribScore, IScoreCalculator scoreCalculator = null)
            : base(eventStream, gameState, playerId, round)
        {
            if (playerCountedCribScore < 0) throw new ArgumentOutOfRangeException("playerCountedCribScore");
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
            PlayerCountedScore = playerCountedCribScore;
        }
    }
}
