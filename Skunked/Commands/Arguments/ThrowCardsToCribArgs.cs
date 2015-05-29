using System;
using System.Collections.Generic;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked.Commands
{
    public class ThrowCardsToCribArgs : CommandArgsBase
    {
        public IEnumerable<Card> CardsToThrow { get; private set; }
        public IScoreCalculator ScoreCalculator { get; private set; }

        public ThrowCardsToCribArgs(GameState gameState, int playerId, int round, IEnumerable<Card> cardsToThrow, IScoreCalculator scoreCalculator = null)
            : base(gameState, playerId, round)
        {
            if (cardsToThrow == null) throw new ArgumentNullException("cardsToThrow");
            CardsToThrow = cardsToThrow;
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }
    }
}
