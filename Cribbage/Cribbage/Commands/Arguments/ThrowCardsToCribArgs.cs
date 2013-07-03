using System;
using System.Collections.Generic;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Score;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments
{
    public class ThrowCardsToCribArgs : CommandArgsBase
    {
        public IEnumerable<ICard> CardsToThrow { get; set; }
        public IScoreCalculator ScoreCalculator { get; set; }

        public ThrowCardsToCribArgs(CribGameState gameState, int playerID, int round, IEnumerable<ICard> cardsToThrow, IScoreCalculator scoreCalculator)
            : base(gameState, playerID, round)
        {
            if (cardsToThrow == null) throw new ArgumentNullException("cardsToThrow");
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            CardsToThrow = cardsToThrow;
            ScoreCalculator = scoreCalculator;
        }
    }
}
