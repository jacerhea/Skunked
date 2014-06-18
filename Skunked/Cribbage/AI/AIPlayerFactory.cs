using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.AI.CardToss;
using Skunked.AI.Count;
using Skunked.AI.Play;
using Skunked.Players;
using Skunked.PlayingCards.Order;
using Skunked.Score;

namespace Skunked.AI
{
    public class AIPlayerFactory : IAIPlayerFactory
    {
        private readonly IPlayStrategy _playStrategy;
        private readonly IDecisionStrategy _decisionStrategy;

        public AIPlayerFactory(IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy)
        {
            if (playStrategy == null) throw new ArgumentNullException("playStrategy");
            if (decisionStrategy == null) throw new ArgumentNullException("decisionStrategy");
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
        }

        public List<Player> CreatePlayers(int numberOfPlayers)
        {
            if (numberOfPlayers < 0) throw new ArgumentOutOfRangeException("numberOfPlayers");

            var players = new List<Player>(numberOfPlayers);
            foreach (var iteration in Enumerable.Range(1, numberOfPlayers))
            {
                var playerName = string.Format("Player {0}", iteration);
                var player = new Player(playerName, iteration, _playStrategy, _decisionStrategy, new PercentageScoreCountStrategy(100, new ScoreCalculator()));
                players.Add(player);
            }

            return players;
        }

        public Player CreatePlayer(AIDifficulty difficulty, string name)
        {
            var standardOrder = new StandardOrder();
            var scoreCalculator = new ScoreCalculator();

            switch (difficulty)
            {
                case AIDifficulty.Easy:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new MinAverageDecision(scoreCalculator), new PercentageScoreCountStrategy(70, scoreCalculator));
                case AIDifficulty.Medium:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new RandomDecision(), new PercentageScoreCountStrategy(80, scoreCalculator));
                case AIDifficulty.Hard:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new OptimisticDecision(), new PercentageScoreCountStrategy(90, scoreCalculator));
                case AIDifficulty.Expert:
                    return new Player(name, -1, new LowestCardPlayStrategy(standardOrder), new MaxAverageDecision(scoreCalculator), new PercentageScoreCountStrategy(100, scoreCalculator));
                default:
                    throw new NotSupportedException("Difficulty type not supported.");
            }
        }
    }
}