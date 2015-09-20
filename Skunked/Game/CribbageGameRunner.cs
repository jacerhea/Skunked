using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Game
{
    public class CribbageGameRunner
    {
        private readonly List<Player> _players;
        private readonly GameRules _gameRules;
        private readonly Deck _deck;

        /// <summary>
        /// Synchronous Game of Cribbage
        /// </summary>
        /// <param name="gameRules">Set of rules the game will abide by.</param>
        /// <param name="players">2-4 players</param>
        /// <param name="deck"></param>
        public CribbageGameRunner(List<Player> players = null, GameRules gameRules = null, Deck deck = null)
        {
            _gameRules = gameRules ?? new GameRules();

            _players = players ?? new List<Player> { new Player("Player 1"), new Player("Player 2") };
            if (_players.Count > 4 || _players.Count < 2) throw new ArgumentOutOfRangeException(nameof(players));
            if (_players.Count != _gameRules.PlayerCount) throw new ArgumentOutOfRangeException(nameof(players));

            _deck = deck ?? new Deck();
        }

        public GameState Run()
        {
            var cribbage = new Cribbage(_players.Select(p => p.Id), _gameRules);
            var gameState = cribbage.State;

            var cardsForCut = _deck.ToList();

            foreach (var player in _players)
            {
                var cutCard = player.ChooseCard(cardsForCut);
                cardsForCut.Remove(cutCard);
                cribbage.CutCard(player.Id, cutCard);
            }

            try
            {
                while (true)
                {
                    var currentRound = gameState.GetCurrentRound();
                    foreach (var playerDiscard in _players)
                    {
                        var tossed = playerDiscard.DealHand(currentRound.DealtCards.Single(p => p.Id == playerDiscard.Id).Hand);
                        cribbage.ThrowCards(playerDiscard.Id, tossed);
                    }

                    while (!currentRound.PlayedCardsComplete)
                    {
                        var currentPlayerPlayItems = currentRound.ThePlay.Last();
                        var lastPlayerPlayItem = currentRound.ThePlay.SelectMany(ppi => ppi).LastOrDefault();
                        Player player = currentRound.ThePlay.Count == 1 && lastPlayerPlayItem == null ? _players.NextOf(_players.Single(p => p.Id == currentRound.PlayerCrib)) : _players.Single(p => p.Id == lastPlayerPlayItem.NextPlayer);
                        var playedCards = currentRound.ThePlay.SelectMany(ppi => ppi).Select(ppi => ppi.Card).ToList();
                        var handLeft = currentRound.Hands.Single(playerHand => playerHand.Id == player.Id).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                        var show = player.PlayShow(_gameRules, currentPlayerPlayItems.Select(y => y.Card).ToList(), handLeft);

                        cribbage.PlayCard(player.Id, show);
                    }

                    var startingPlayer = _players.Single(p => p.Id == gameState.GetNextPlayerFrom(currentRound.PlayerCrib));
                    foreach (var player in _players.Infinite().Skip(_players.IndexOf(startingPlayer)).Take(_players.Count).ToList())
                    {
                        var playerCount = player.CountHand(currentRound.Starter, currentRound.Hands.Single(kv => kv.Id == player.Id).Hand);
                        cribbage.CountHand(player.Id, playerCount);
                    }

                    var cribCount = _players.Single(p => p.Id == currentRound.PlayerCrib).CountHand(currentRound.Starter, currentRound.Crib);

                    cribbage.CountCrib(currentRound.PlayerCrib, cribCount);
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