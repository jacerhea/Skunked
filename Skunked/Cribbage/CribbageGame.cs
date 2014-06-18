using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.Commands.Arguments;
using Skunked.Dealer;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score.Interface;
using Skunked.State;

namespace Skunked
{
    public class CribbageGame
    {
        private readonly List<Player> _players;
        private readonly Dictionary<Player, PlayerScore> _playerScoreLookup;
        private readonly GameRules _gameRules;
        private readonly Deck _deck;

        private readonly IScoreCalculator _scoreCalculator;
        private readonly IPlayerHandFactory _handFactory;

        /// <summary>
        /// Game of Cribbage, only supports 2 or 4 players currently.
        /// </summary>
        public CribbageGame(GameRules gameRules = null, List<Player> players = null, Deck deck = null, IScoreCalculator scoreCalculator = null, IPlayerHandFactory handFactory = null)
        {
            //if (deck == null) throw new ArgumentNullException("deck");
            //if (scoreCalculator == null) throw new ArgumentNullException("scoreCalculator");
            //if (handFactory == null) throw new ArgumentNullException("handFactory");

            _gameRules = gameRules ?? new GameRules();

            _players = players ?? new List<Player> { new Player("Player 1"), new Player("Player 2") };
            if (players.Count > 4 || players.Count < 2) throw new ArgumentOutOfRangeException("players");
            //var gameScoresToCreate = _players.Count == 3 ? 3 : 2;
            //var gameScores = Enumerable.Range(1, gameScoresToCreate).Select(x => new GameScore(0)).ToList();

            //_playerScoreLookup = new Dictionary<Player, PlayerScore>(_players.Count);
            //for (int i = 0; i < _players.Count; i++)
            //{
            //    var indexOfGameScore = _players.Count == 3 ? i : i % 2;
            //    var playerScore = new PlayerScore(_players[i], gameScores[indexOfGameScore]);

            //    PlayerScoreLookup.Add(_players[i], playerScore);
            //}

            _deck = deck ?? new Deck();
            //_scoreCalculator = scoreCalculator;
            _handFactory = handFactory ?? new StandardHandDealer();
        }

        public Dictionary<Player, PlayerScore> PlayerScoreLookup
        {
            get { return _playerScoreLookup; }
        }

        public GameState Run()
        {
            //GameState = new CribGameState
            //                {
            //                    GameRules = _gameRules,
            //                    Players =
            //                        _players.Select(cp => new SerializablePlayer {ID = cp.ID, Name = cp.Name}).ToList(),
            //                        Rounds = new List<CribRoundState>(),
            //                        OpeningRoundState = new CribOpeningRoundState{IsDone = true, WinningPlayerCut = }
            //                };
            //GameState.PlayerScores = GameState.Players.Select(sp => new SerializablePlayerScore{Player = sp.ID, Score = 0}).ToList();

            var gameState = new GameState();
            var createGame = new CreateCribbageGameStateCommand(_players, gameState, _gameRules);
            createGame.Execute();

            var cardsForCut = _deck.Cards.ToList();

            foreach (var player in _players)
            {
                var cutCard = player.ChooseCard(cardsForCut);
                cardsForCut.Remove(cutCard);
                var command = new CutCardCommand(new CutCardArgs(gameState, player.Id, 0, cutCard));
                command.Execute();
            }

            //var winningPlayerCut = _theCut.GetWinningCut(_players, _deck);
            //var dealer = winningPlayerCut.Player;

            while (true)
            {
                var roundHands = _handFactory.CreatePlayerHands(_deck, _players.ToList(), _gameRules.HandSizeToDeal);

                var crib = new List<Card>(_gameRules.HandSize);

                foreach (var playerDiscard in _players.Select(p => new {Player = p, ThrowToCrib = p.DealHand(roundHands[p])}))
                {
                    playerDiscard.ThrowToCrib.ForEach(c => roundHands[playerDiscard.Player].Remove(c));
                    crib.AddRange(playerDiscard.ThrowToCrib);
                }

                ////card cut
                //var cardsNotDealt = _deck.Cards.Skip(_gameRules.HandSizeToDeal * _players.Count).ToList();
                //var starterCard = _players.NextOf(dealer).ChooseCard(cardsNotDealt);
                //PlayerScoreLookup[dealer].Score.AddPoints(_scoreCalculator.CountCut(starterCard));

                ////the play
                //_thePlay.Execute(GameState, _players, PlayerScoreLookup, roundHands, dealer);

                ////the show
                //_theShow.Execute(_players, PlayerScoreLookup, roundHands, dealer, crib, starterCard);
                //change dealers for next round
                //dealer = _players.NextOf(dealer);
            }

            return gameState;
        }

        //private void ScoreChanged(object sender, GameScoreEventArgs e)
        //{
        //    var ps = sender as PlayerScore;
        //    if (ps != null && ps.Score.Value >= _gameRules.WinningScore)
        //    {
        //        throw new GameException(GameExceptionTypes.Won);
        //    }
        //}

    }
}