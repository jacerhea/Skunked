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
    public class CreateCribbageGameStateCommand : ICommand
    {
        private readonly IEnumerable<Player> _players;
        private readonly GameRules _rules;
        private readonly GameState _gameState;

        public CreateCribbageGameStateCommand(IEnumerable<Player> players, GameState gameState, GameRules rules)
        {
            if (players == null) throw new ArgumentNullException("players");
            if (gameState == null) throw new ArgumentNullException("gameState");
            if (rules == null) throw new ArgumentNullException("rules");
            _players = players;
            _gameState = gameState;
            _rules = rules;
        }

        public void Execute()
        {
            var deck = EnumHelper.GetValues<Rank>().Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new Card(rank, suit)).ToList();
            deck.Shuffle();
            DateTimeOffset now = DateTimeOffset.Now;
            _gameState.Rules = _rules;
            _gameState.OpeningRound = new OpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 Complete = false,
                                                                 CutCards = new List<CustomKeyValuePair<int, Card>>(),
                                                                 WinningPlayerCut = null
                                                             };
            _gameState.Rounds = new List<RoundState>();
            _gameState.Scores = new List<PlayerScore>(_players.Select(player => new PlayerScore { Player = player.Id, Score = 0 }));
            _gameState.Players = _players.Select(p => new Player(p.Name, p.Id)).ToList();
            _gameState.StartedAt = now;
            _gameState.LastUpdated = now;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
