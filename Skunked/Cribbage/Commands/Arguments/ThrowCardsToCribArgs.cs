using System;
using System.Collections.Generic;
using Skunked.Score.Interface;
using Skunked.State;

namespace Skunked.Commands.Arguments
{
    public class ThrowCardsToCribArgs : CommandArgsBase
    {
        public IEnumerable<Card> CardsToThrow { get; set; }
        public IScoreCalculator ScoreCalculator { get; set; }

        public ThrowCardsToCribArgs(GameState gameState, int playerId, int round, IEnumerable<Card> cardsToThrow, IScoreCalculator scoreCalculator)
            : base(gameState, playerId, round)
        {
            if (cardsToThrow == null) throw new ArgumentNullException("cardsToThrow");
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            CardsToThrow = cardsToThrow;
            ScoreCalculator = scoreCalculator;
        }
    }
}
