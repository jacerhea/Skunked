using System;
using Cribbage.Score.Interface;
using Cribbage.State;
using Skunked;

namespace Cribbage.Commands.Arguments
{
    public class PlayCardArgs : CommandArgsBase
    {
        public Card PlayedCard { get; private set; }
        public IScoreCalculator ScoreCalculator { get; private set; }

        public PlayCardArgs(CribGameState gameState, int playerID, int round, Card playedCard, IScoreCalculator scoreCalculator) : base(gameState, playerID, round)
        {
            if (playedCard == null) throw new ArgumentNullException("playedCard");
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            PlayedCard = playedCard;
            ScoreCalculator = scoreCalculator;
        }
    }
}
