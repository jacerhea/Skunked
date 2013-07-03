using System;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Score;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments
{
    public class PlayCardArgs : CommandArgsBase
    {
        public ICard PlayedCard { get; private set; }
        public IScoreCalculator ScoreCalculator { get; private set; }

        public PlayCardArgs(CribGameState gameState, int playerID, int round, ICard playedCard, IScoreCalculator scoreCalculator) : base(gameState, playerID, round)
        {
            if (playedCard == null) throw new ArgumentNullException("playedCard");
            if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            PlayedCard = playedCard;
            ScoreCalculator = scoreCalculator;
        }
    }
}
