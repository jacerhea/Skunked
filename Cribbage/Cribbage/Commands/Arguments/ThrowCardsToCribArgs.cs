using System;
using System.Collections.Generic;
using Cribbage.Score.Interface;
using Cribbage.State;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;

namespace Cribbage.Commands.Arguments
{
    public class ThrowCardsToCribArgs : CommandArgsBase
    {
        public IEnumerable<Card> CardsToThrow { get; set; }
        public IScoreCalculator ScoreCalculator { get; set; }

        public ThrowCardsToCribArgs(CribGameState gameState, int playerID, int round, IEnumerable<Card> cardsToThrow, IScoreCalculator scoreCalculator)
            : base(gameState, playerID, round)
        {
            if (cardsToThrow == null) throw new ArgumentNullException("cardsToThrow");
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            CardsToThrow = cardsToThrow;
            ScoreCalculator = scoreCalculator;
        }
    }
}
