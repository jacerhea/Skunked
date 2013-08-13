using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage;
using Cribbage.Rules;
using Cribbage.State;
using Cribbage.Utility;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands
{
    public class CreateCribbageGameStateCommand : ICommand
    {
        private readonly IEnumerable<IPlayer> _players;
        private readonly CribGameRules _rules;
        private CribGameState _cribGameState;

        public CribGameState CribGameState
        {
            get
            {
                return _cribGameState;
            }
        }

        public CreateCribbageGameStateCommand(IEnumerable<IPlayer> players, CribGameRules rules)
        {
            if (players == null) throw new ArgumentNullException("players");
            _players = players;
            _rules = rules;
        }

        public void Execute()
        {
            var deck = EnumHelper.GetValues<Rank>().SelectMany(rank => EnumHelper.GetValues<Suit>().Select(suit => new SerializableCard { Rank = rank, Suit = suit })).ToList();
            deck.Shuffle();
            _cribGameState = new CribGameState
                                 {
                                     GameRules = _rules,
                                     OpeningRoundState = new CribOpeningRoundState
                                                             {
                                                                 Deck = deck,
                                                                 IsDone = false,
                                                                 PlayersCutCard =new List<SerializableKeyValuePair<int, SerializableCard>>(),
                                                                 WinningPlayerCut = null
                                                             },
                                     Rounds = new List<CribRoundState>(),
                                     PlayerScores = new List<SerializablePlayerScore>(_players.Select(player => new SerializablePlayerScore { Player = player.ID, Score = 0 })),
                                     Players = _players.Select(p => new SerializablePlayer { ID = p.ID, Name = p.Name }).ToList(),
                                     StartedAt = DateTime.Now,
                                     LastUpdated = DateTime.Now
                                 };
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
