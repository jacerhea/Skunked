using System;
using Skunked.PlayingCards;
using Skunked.Score;
using Skunked.State;

namespace Skunked.Commands
{
    public class PlayCardArgs : CommandArgsBase
    {
        public Card PlayedCard { get; private set; }
        public IScoreCalculator ScoreCalculator { get; private set; }

        public PlayCardArgs(GameState gameState, int playerId, int round, Card playedCard, IScoreCalculator scoreCalculator = null) : base(gameState, playerId, round)
        {
            if (playedCard == null) throw new ArgumentNullException("playedCard");
            PlayedCard = playedCard;
            ScoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }
    }
}
