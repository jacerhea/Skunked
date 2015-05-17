using System;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public class CountHandScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountHandScoreArgs(GameEventStream eventStream, GameState gameState, int playerId, int round, int playerCountedScore, IScoreCalculator scoreCalculator = null)
            : base(eventStream, gameState, playerId, round)
        {
            if (playerCountedScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
            PlayerCountedScore = playerCountedScore;
        }
    }
}
