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
        private GameState _gameState;

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
        }

        public CreateCribbageGameStateCommand(IEnumerable<Player> players, GameRules rules)
        {
            if (players == null) throw new ArgumentNullException("players");
            _players = players;
            _rules = rules;
        }

        public void Execute()
        {
            var deck = EnumHelper.GetValues<Rank>().Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new Card(rank, suit)).ToList();
            deck.Shuffle();
            DateTime now = DateTime.Now;
            _gameState = new GameState
                                 {
                                     GameRules = _rules,
                                     OpeningRoundState = new OpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 IsDone = false,
                                                                 PlayersCutCard = new List<CustomKeyValuePair<int, Card>>(),
                                                                 WinningPlayerCut = null
                                                             },
                                     Rounds = new List<RoundState>(),
                                     PlayerScores = new List<PlayerScore>(_players.Select(player => new PlayerScore { Player = player.Id, Score = 0 })),
                                     Players = _players.Select(p => new Player (p.Name, p.Id)).ToList(),
                                     StartedAt = now,
                                     LastUpdated = now
                                 };
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
