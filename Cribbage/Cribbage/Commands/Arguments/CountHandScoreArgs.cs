using System;
using Cribbage.Score.Interface;
using Cribbage.State;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;

namespace Cribbage.Commands.Arguments
{
    public class CountHandScoreArgs : CommandArgsBase
    {
        public IScoreCalculator ScoreCalculator { get; private set; }
        public int PlayerCountedScore { get; private set; }

        public CountHandScoreArgs(CribGameState cribGameState, int playerID, int round, IScoreCalculator scoreCalculator, int playerCountedScore)
            : base(cribGameState, playerID, round)
        {
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            if (playerCountedScore < 0) throw new ArgumentOutOfRangeException("playerCountedScore");
            ScoreCalculator = scoreCalculator;
            PlayerCountedScore = playerCountedScore;
        }
    }
}
