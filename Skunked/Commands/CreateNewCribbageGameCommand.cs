using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CreateNewCribbageGameCommand : ICommand
    {
        private readonly List<Player> _players;
        private readonly GameRules _gameRules;
        private readonly GameState _gameState;

        public CreateNewCribbageGameCommand(IEnumerable<Player> players, GameState gameState, GameRules gameRules)
        {
            if (players == null) throw new ArgumentNullException("players");
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (gameRules == null) throw new ArgumentNullException("gameRules");
            _players = players.ToList();
            _gameState = gameState;
            _gameRules = gameRules;
        }

        public void Execute()
        {
            var deck = new Deck().ToList();
            deck.Shuffle();
            DateTimeOffset now = DateTimeOffset.Now;
            _gameState.GameRules = _gameRules;
            _gameState.OpeningRound = new OpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 Complete = false,
                                                                 CutCards = new List<CustomKeyValuePair<int, Card>>(),
                                                                 WinningPlayerCut = null
                                                             };
            _gameState.Rounds = new List<RoundState>();
            _gameState.IndividualScores = new List<PlayerScore>(_players.Select(player => new PlayerScore { Player = player.Id, Score = 0 }));
            _gameState.Players = _players.Select(p => new Player(p.Name, p.Id)).ToList();
            _gameState.TeamScores = _players.Count == 2
                ? _players.Select(p => new TeamScore {Players = new List<int> {p.Id}}).ToList()
                : new List<TeamScore>
                {
                    new TeamScore {Players = new List<int> {_players[0].Id, _players[2].Id}},
                    new TeamScore {Players = new List<int> {_players[1].Id, _players[3].Id}}
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
