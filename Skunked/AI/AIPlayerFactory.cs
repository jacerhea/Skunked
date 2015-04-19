using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Score;

namespace Skunked.AI
{
    public class AiPlayerFactory : IAiPlayerFactory
    {
        private readonly IPlayStrategy _playStrategy;
        private readonly IDecisionStrategy _decisionStrategy;

        public AiPlayerFactory(IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy)
        {
            if (playStrategy == null) throw new ArgumentNullException("playStrategy");
            if (decisionStrategy == null) throw new ArgumentNullException("decisionStrategy");
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
        }

        public List<Player> CreatePlayers(int numberOfPlayers)
        {
            if (numberOfPlayers < 0) throw new ArgumentOutOfRangeException("numberOfPlayers");

            return Enumerable.Range(1, numberOfPlayers)
                    .Select(iteration => new {iteration, playerName = string.Format("Player {0}", iteration)})
                    .Select(t => new Player(t.playerName, t.iteration, _playStrategy, _decisionStrategy,new PercentageScoreCountStrategy()))
                    .ToList();
        }

        public Player CreatePlayer(AiDifficulty difficulty, string name)
        {
            var standardOrder = new StandardOrder();
            var scoreCalculator = new ScoreCalculator();

            switch (difficulty)
            {
                case AiDifficulty.Easy:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new MinAverageDecision(scoreCalculator), new PercentageScoreCountStrategy(70, scoreCalculator));
                case AiDifficulty.Medium:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new RandomDecision(), new PercentageScoreCountStrategy(80, scoreCalculator));
                case AiDifficulty.Hard:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new OptimisticDecision(), new PercentageScoreCountStrategy(90, scoreCalculator));
                case AiDifficulty.Expert:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new MaxAverageDecision(scoreCalculator), new PercentageScoreCountStrategy(100, scoreCalculator));
                default:
                    throw new NotSupportedException("Difficulty type not supported.");
            }
        }
    }
}