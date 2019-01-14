﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Game
{
    public class CribbageGameRunner
    {
        private readonly Deck _deck;

        /// <summary>
        /// Synchronous Game of Cribbage
        /// </summary>
        /// <param name="deck"></param>
        public CribbageGameRunner(Deck deck)
        {
            _deck = deck;
        }

        public Cribbage Run(List<IGameRunnerPlayer> players, GameRules gameRules)
        {
            if (players.Count > 4 || players.Count < 2) throw new ArgumentOutOfRangeException(nameof(players));
            if (players.Count != gameRules.PlayerCount) throw new ArgumentOutOfRangeException(nameof(players));

            var cribbage = new Cribbage(players.Select(p => p.Id), gameRules);
            var gameState = cribbage.State;

            var cardsForCut = _deck.ToList();

            foreach (var player in players)
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
                    foreach (var playerDiscard in players)
                    {
                        var tossed = playerDiscard.DealHand(currentRound.DealtCards.Single(p => p.PlayerId == playerDiscard.Id).Hand);
                        cribbage.ThrowCards(playerDiscard.Id, tossed);
                    }

                    while (!currentRound.PlayedCardsComplete)
                    {
                        var currentPlayerPlayItems = currentRound.ThePlay.Last();
                        var lastPlayerPlayItem = currentRound.ThePlay.SelectMany(ppi => ppi).LastOrDefault();
                        var player = currentRound.ThePlay.Count == 1 && lastPlayerPlayItem == null
                            ? players.NextOf(players.Single(p => p.Id == currentRound.PlayerCrib))
                            : players.Single(p => p.Id == lastPlayerPlayItem.NextPlayer);
                        var playedCards = currentRound.ThePlay.SelectMany(ppi => ppi).Select(ppi => ppi.Card).ToList();
                        var handLeft = currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                        var show = player.PlayShow(gameRules, currentPlayerPlayItems.Select(y => y.Card).ToList(), handLeft);

                        cribbage.PlayCard(player.Id, show);
                    }

                    var startingPlayer = players.Single(p => p.Id == gameState.GetNextPlayerFrom(currentRound.PlayerCrib));
                    foreach (var player in players.Infinite().Skip(players.IndexOf(startingPlayer)).Take(players.Count).ToList())
                    {
                        var playerCount = player.CountHand(currentRound.Starter, currentRound.Hands.Single(kv => kv.PlayerId == player.Id).Hand);
                        cribbage.CountHand(player.Id, playerCount);
                    }

                    var cribCount = players.Single(p => p.Id == currentRound.PlayerCrib).CountHand(currentRound.Starter, currentRound.Crib);

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


            return cribbage;
        }
    }
}