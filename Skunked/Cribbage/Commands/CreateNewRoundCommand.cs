using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Commands;
using Cribbage.Dealer;
using Cribbage.State;
using Skunked.Dealer;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CreateNewRoundCommand : ICommand
    {
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
            var deck = new Deck();
            var playerHandFactory = new StandardHandDealer();
            var players = _gameState.Players.Cast<Player>().ToList();

            var playerHands = playerHandFactory.CreatePlayerHands(deck, players, _gameState.GameRules.HandSizeToDeal);

            var serializedPlayerHands = playerHands.Select( kv => new CustomKeyValuePair<int, List<Card>>
                    {
                        Key = kv.Key.ID,
                        Value = kv.Value.Select(
                        c => new Card(c)).
                        ToList()
                    }).ToList();

            var playerShowScores = new List<PlayerScoreShow>(_gameState.Players.Select(sp => new PlayerScoreShow { CribScore = null, HasShowed = false, Player = sp.ID, PlayerCountedShowScore = 0, ShowScore = 0}));

            int cribPlayerID;
            if(_gameState.OpeningRoundState.IsDone && _gameState.Rounds.Count != 0)
            {
                cribPlayerID = _gameState.Players.NextOf(_gameState.Players.Single(sp => _gameState.Rounds.Single(r => r.Round == _currentRound).PlayerCrib == sp.ID)).ID;
            }   
            else
            {
                cribPlayerID = _gameState.OpeningRoundState.WinningPlayerCut.Value;
            }


            var roundState = new RoundState
            {
                Crib = new List<Card>(),
                PlayerDealtCards = serializedPlayerHands,
                IsDone = false,
                PlayerCrib = cribPlayerID,
                PlayerHand = new List<CustomKeyValuePair<int, List<Card>>>(),
                PlayersShowedCards = new List<List<PlayerPlayItem>> { new List<PlayerPlayItem>() },
                Round = _currentRound + 1,
                PlayerShowScores = playerShowScores
            };

            _gameState.Rounds.Add(roundState);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
