using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;

namespace Skunked.Commands
{
    public class CreateNewCribbageGameCommand : ICommand
    {
        private readonly List<int> _players;
        private readonly GameRules _gameRules;
        private readonly GameState _gameState;

        public CreateNewCribbageGameCommand(GameState gameState, IEnumerable<int> players, GameRules gameRules)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));
            if (gameState == null) throw new ArgumentNullException(nameof(gameState));
            if (gameRules == null) throw new ArgumentNullException(nameof(gameRules));
            _players = players.ToList();
            _gameState = gameState;
            _gameRules = gameRules;
        }

        public void Execute()
        {
            var deck = new Deck().ToList();
            DateTimeOffset now = DateTimeOffset.Now;
            _gameState.GameRules = _gameRules;
            _gameState.OpeningRound = new OpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 Complete = false,
                                                                 CutCards = new List<PlayerIdCard>(),
                                                                 WinningPlayerCut = null
                                                             };
            _gameState.Rounds = new List<RoundState>();
            _gameState.IndividualScores = new List<PlayerScore>(_players.Select(player => new PlayerScore { Player = player, Score = 0 }));
            _gameState.PlayerIds = _players.ToList();
            _gameState.TeamScores = _players.Count == 2
                ? _players.Select(p => new TeamScore {Players = new List<int> {p}}).ToList()
                : new List<TeamScore>
                {
                    new TeamScore {Players = new List<int> {_players[0], _players[2]}},
                    new TeamScore {Players = new List<int> {_players[1], _players[3]}}
                };
            _gameState.StartedAt = now;
            _gameState.LastUpdated = now;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
