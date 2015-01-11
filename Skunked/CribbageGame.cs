using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.Commands.Arguments;
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
        private readonly GameRules _gameRules;
        private readonly Deck _deck;

        private readonly IScoreCalculator _scoreCalculator;

        /// <summary>
        /// Synchronous Game of Cribbage
        /// </summary>
        /// <param name="gameRules">Set of rules the game will abide by.</param>
        /// <param name="players">2-4 players</param>
        /// <param name="deck"></param>
        /// <param name="scoreCalculator"></param>
        public CribbageGame(List<Player> players = null, GameRules gameRules = null, Deck deck = null, IScoreCalculator scoreCalculator = null)
        {
            _gameRules = gameRules ?? new GameRules();

            _players = players ?? new List<Player> { new Player("Player 1"), new Player("Player 2") };
            if (_players.Count > 4 || _players.Count < 2 ) throw new ArgumentOutOfRangeException("players");
            if (_players.Count != _gameRules.PlayerCount) throw new ArgumentOutOfRangeException("players");

            _deck = deck ?? new Deck();
            _scoreCalculator = scoreCalculator ?? new ScoreCalculator();
        }

        public GameState Run()
        {
            var gameState = new GameState();
            var createGame = new CreateCribbageGameStateCommand(_players, gameState, _gameRules);
            createGame.Execute();

            var cardsForCut = _deck.ToList();

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
                    var currentRound = gameState.GetCurrentRound();
                    foreach (var playerDiscard in _players)
                    {
                        var tossed = playerDiscard.DealHand(currentRound.DealtCards.Single(p => p.Id == playerDiscard.Id).Hand);
                        var throwToCribCommand = new ThrowCardsToCribCommand(new ThrowCardsToCribArgs(gameState, playerDiscard.Id,
                                currentRound.Round, tossed, _scoreCalculator));
                        throwToCribCommand.Execute();
                    }

                    while (!currentRound.PlayedCardsComplete)
                    {
                        var currentPlayerPlayItems = currentRound.ThePlay.Last();
                        var lastPlayerPlayItem = currentRound.ThePlay.SelectMany(ppi => ppi).LastOrDefault();
                        Player player = currentRound.ThePlay.Count == 1 && lastPlayerPlayItem == null ? _players.NextOf(_players.Single(p => p.Id == currentRound.PlayerCrib)) : _players.Single(p => p.Id == lastPlayerPlayItem.NextPlayer);
                        var playedCards = currentRound.ThePlay.SelectMany(ppi => ppi).Select(ppi => ppi.Card).ToList();
                        var handLeft = currentRound.Hands.Single(playerHand => playerHand.Id == player.Id).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                        var show = player.PlayShow(_gameRules, currentPlayerPlayItems.Select(y => y.Card).ToList(), handLeft);
                        var command = new PlayCardCommand(new PlayCardArgs(gameState, player.Id, currentRound.Round, show, _scoreCalculator));
                        command.Execute();
                    }

                    var startingPlayer = gameState.GetNextPlayerFrom(currentRound.PlayerCrib);
                    foreach (var player in _players.Infinite().Skip(_players.IndexOf(startingPlayer)).Take(_players.Count).ToList())
                    {
                        var playerCount = player.CountHand(currentRound.Starter, currentRound.Hands.Single(kv => kv.Id == player.Id).Hand);
                        var countCommand = new CountHandScoreCommand(new CountHandScoreArgs(gameState, player.Id, currentRound.Round, _scoreCalculator, playerCount));
                        countCommand.Execute();                            
                    }


                    var cribCount = _players.Single(p => p.Id == currentRound.PlayerCrib).CountHand(currentRound.Starter, currentRound.Crib);
                    var countCribCommand = new CountCribScoreCommand(new CountCribScoreArgs(gameState, currentRound.PlayerCrib, currentRound.Round, _scoreCalculator, cribCount));
                    countCribCommand.Execute();
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