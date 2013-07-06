using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Rules;
using Cribbage.State;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.State;
using Games.Domain.MainModule.Entities.CardGames.Player;
using Games.Domain.MainModule.Entities.PlayingCards;
using Games.Infrastructure.CrossCutting;
using Games.Infrastructure.CrossCutting.Collections;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands
{
    public class CreateCribbageGameStateCommand : ICommand
    {
        private readonly IEnumerable<IPlayer> _players;
        private readonly CribGameRules _rules;
        private CribGameState _cribGameState;
        private EnumEnumerator _enumIterator;

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
            _enumIterator = new EnumEnumerator();
        }

        public void Execute()
        {
            var deck = _enumIterator.GetEnumerator<Rank>().SelectMany(rank => _enumIterator.GetEnumerator<Suit>().Select(suit => new SerializableCard { Rank = rank, Suit = suit })).ToList();
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
