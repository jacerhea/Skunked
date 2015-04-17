using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Dealer;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CreateNewRoundCommand : ICommand
    {
        private readonly IPlayerHandFactory _playerHandFactory = new StandardHandDealer();
        private readonly GameState _gameState;
        private readonly int _currentRound;

        public CreateNewRoundCommand(GameState gameState, int currentRound)
        {
            if (gameState == null) throw new ArgumentNullException("gameState");
            _gameState = gameState;
            _currentRound = currentRound;
        }

        public void Execute()
        {
            var playerShowScores = new List<PlayerScoreShow>(_gameState.Players.Select(sp => new PlayerScoreShow { CribScore = null, HasShowed = false, Player = sp.Id, PlayerCountedShowScore = 0, ShowScore = 0}));

            int cribPlayerId;
            if(_gameState.OpeningRound.Complete && _gameState.Rounds.Count != 0)
            {
                cribPlayerId = _gameState.Players.NextOf(_gameState.Players.Single(sp => _gameState.Rounds.Single(r => r.Round == _currentRound).PlayerCrib == sp.Id)).Id;
            }   
            else
            {
                cribPlayerId = _gameState.OpeningRound.WinningPlayerCut.Value;
            }

            var deck = new Deck();
            var players = _gameState.Players.ToList();

            var playerHands = _playerHandFactory.CreatePlayerHands(deck, players, players[0], _gameState.GameRules.HandSizeToDeal);

            var serializedPlayerHands = playerHands.Select(kv => new PlayerIdHand(kv.Key.Id, kv.Value.Select(c => new Card(c)).ToList())).ToList();

            var roundState = new RoundState
            {
                Crib = new List<Card>(),
                DealtCards = serializedPlayerHands,
                Complete = false,
                PlayerCrib = cribPlayerId,
                Hands = new List<PlayerIdHand>(),
                ThePlay = new List<List<PlayerPlayItem>> { new List<PlayerPlayItem>() },
                Round = _currentRound + 1,
                ShowScores = playerShowScores
            };

            _gameState.Rounds.Add(roundState);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
