using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.Commands.Arguments;
using Skunked.Dealer;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.Score.Interface;
using Skunked.State;
using Skunked.Utility;

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
            //if (handFactory == null) throw new ArgumentNullException("handFactory");

            _gameRules = gameRules ?? new GameRules();

            _players = players ?? new List<Player> { new Player("Player 1"), new Player("Player 2") };
            if (_players.Count > 4 || _players.Count < 2) throw new ArgumentOutOfRangeException("players");
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
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
            _handFactory = handFactory ?? new StandardHandDealer();
        }

        public Dictionary<Player, PlayerScore> PlayerScoreLookup
        {
            get { return _playerScoreLookup; }
        }

        public GameState Run()
        {
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

            try
            {
                while (true)
                {
                    var roundHands = _handFactory.CreatePlayerHands(_deck, _players.ToList(), _gameRules.HandSizeToDeal);

                    var round = gameState.GetCurrentRound().Round;
                    foreach (var playerDiscard in _players.Select(p => new { Player = p, ThrowToCrib = p.DealHand(roundHands[p]) }))
                    {
                        playerDiscard.ThrowToCrib.ForEach(c => roundHands[playerDiscard.Player].Remove(c));
                        var throwToCribCommand = new ThrowCardsToCribCommand(new ThrowCardsToCribArgs(gameState, playerDiscard.Player.Id,
                                round, playerDiscard.ThrowToCrib, _scoreCalculator));
                        throwToCribCommand.Execute();
                    }

                    while (!gameState.GetCurrentRound().PlayCardsIsDone)
                    {
                        var currentPlayerPlayItems = gameState.GetCurrentRound().PlayersShowedCards.Last();
                        var ppi = currentPlayerPlayItems.Last();
                        var player = _players.Single(p => p.Id == ppi.Player);
                        var jkflasdkdf = currentPlayerPlayItems.Select(p => p.Card);
                        var command = new PlayCardCommand(new PlayCardArgs(gameState, player.Id, round, 
                            player.PlayShow(_gameRules, currentPlayerPlayItems.Select(y => y.Card).ToList(), jkflasdkdf.ToList()), _scoreCalculator));
                        command.Execute();
                    }

                    foreach (var player in _players)
                    {
                        player.CountHand(gameState.GetCurrentRound().StartingCard, gameState.GetCurrentRound().PlayerHand.Single(kv => kv.Key == player.Id).Value);
                    }
                }
            }
            catch (InvalidCribbageOperationException exception)
            {
                if (exception.Operation != InvalidCribbageOperations.GameFinished)
                {
                    throw;
                }
            }

            return gameState;
        }
    }
}