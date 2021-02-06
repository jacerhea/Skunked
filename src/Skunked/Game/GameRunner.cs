using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;
using Skunked.Domain.Commands;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Game
{
    /// <summary>
    /// Runs a game of cribbage with the given player strategies provided.
    /// Used for testing, AI comparison, or any other need to run through many games automatically.
    /// </summary>
    public class GameRunner
    {
        private readonly Deck _deck;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRunner"/> class.
        /// </summary>
        /// <param name="deck">The deck to use.</param>
        public GameRunner(Deck deck)
        {
            _deck = deck;
        }

        /// <summary>
        /// Run a game of cribbage using the provided <see cref="IGameRunnerPlayer"/>.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <param name="winningScore">The winning score.</param>
        /// <returns>The game of cribbage after it has completed.</returns>
        public Cribbage Run(List<IGameRunnerPlayer> players, WinningScore winningScore)
        {
            if (players.Count > 4 || players.Count < 2) throw new ArgumentOutOfRangeException(nameof(players));

            var gameRules = new GameRules(winningScore);

            var cribbage = new Cribbage(players.Select(p => p.Id), gameRules);
            var gameState = cribbage.State;

            var cardsForCut = _deck.ToList();

            foreach (var player in players)
            {
                var cutCard = player.CutCards(cardsForCut);
                cardsForCut.Remove(cutCard);
                cribbage.CutCard(new CutCardCommand(player.Id, cutCard));
            }

            try
            {
                while (true)
                {
                    var currentRound = gameState.GetCurrentRound();
                    foreach (var playerDiscard in players)
                    {
                        var tossed = playerDiscard.DetermineCardsToThrow(currentRound.DealtCards.Single(p => p.PlayerId == playerDiscard.Id).Hand);
                        cribbage.ThrowCards(new ThrowCardsCommand(playerDiscard.Id, tossed));
                    }

                    while (!currentRound.PlayedCardsComplete)
                    {
                        var currentPlayerPlayItems = currentRound.ThePlay.Last();
                        var lastPlayerPlayItem = currentRound.ThePlay.SelectMany(ppi => ppi).LastOrDefault();
                        var isFirstPlay = currentRound.ThePlay.Count == 1 && lastPlayerPlayItem == null;
                        var player = isFirstPlay
                            ? players.NextOf(players.Single(p => p.Id == currentRound.PlayerCrib))
                            : players.Single(p => p.Id == lastPlayerPlayItem.NextPlayer);
                        var playedCards = currentRound.ThePlay.SelectMany(ppi => ppi).Select(ppi => ppi.Card).ToList();
                        var handLeft = currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand.Except(playedCards).ToList();
                        var show = player.DetermineCardsToPlay(gameRules, currentPlayerPlayItems.Select(playItem => playItem.Card).ToList(), handLeft);

                        cribbage.PlayCard(new PlayCardCommand(player.Id, show));
                    }

                    var startingPlayer = players.Single(player => player.Id == gameState.GetNextPlayerFrom(currentRound.PlayerCrib));
                    foreach (var player in players.Infinite().Skip(players.IndexOf(startingPlayer)).Take(players.Count).ToList())
                    {
                        var playerCount = player.CountHand(currentRound.Starter, currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand);
                        cribbage.CountHand(new CountHandCommand(player.Id, playerCount));
                    }

                    var cribCount = players.Single(p => p.Id == currentRound.PlayerCrib).CountHand(currentRound.Starter, currentRound.Crib);

                    cribbage.CountCrib(new CountCribCommand(currentRound.PlayerCrib, cribCount));
                }
            }
            catch (GameFinishedException)
            {
            }

            return cribbage;
        }
    }
}