﻿using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Utility;

namespace Skunked.Dealer
{
    /// <summary>
    /// Standard dealer.  One card per pass.
    /// </summary>
    public class StandardHandDealer : IPlayerHandFactory
    {
        //make the startingWith into the dealer.  this saves a step for the caller to figure out who needs to be dealt to first.
        public Dictionary<Player, List<Card>> CreatePlayerHands(Deck deck, IList<Player> players, Player startingWith, int handSize)
        {
            deck.Shuffle();
            var startingIndex = players.IndexOf(startingWith);
            var playersOrdered = players.Infinite().Skip(startingIndex).Take(players.Count).ToList();
            return players.ToDictionary(p => p, p => deck.Cards.Skip(playersOrdered.IndexOf(p)).TakeEvery(2).Take(handSize).ToList());
        }
    }
}