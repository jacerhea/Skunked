using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Players;
using Cribbage.Rules;
using Cribbage.State;
using Cribbage.Utility;

namespace Cribbage.Commands
{
    public class CreateCribbageGameStateCommand : ICommand
    {
        private readonly IEnumerable<Player> _players;
        private readonly CribGameRules _rules;
        private CribGameState _cribGameState;

        public CribGameState CribGameState
        {
            get
            {
                return _cribGameState;
            }
        }

        public CreateCribbageGameStateCommand(IEnumerable<Player> players, CribGameRules rules)
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
            _cribGameState = new CribGameState
                                 {
                                     GameRules = _rules,
                                     OpeningRoundState = new CribOpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 IsDone = false,
                                                                 PlayersCutCard = new List<SerializableKeyValuePair<int, Card>>(),
                                                                 WinningPlayerCut = null
                                                             },
                                     Rounds = new List<CribRoundState>(),
                                     PlayerScores = new List<SerializablePlayerScore>(_players.Select(player => new SerializablePlayerScore { Player = player.ID, Score = 0 })),
                                     Players = _players.Select(p => new Player (p.Name, p.ID)).ToList(),
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
