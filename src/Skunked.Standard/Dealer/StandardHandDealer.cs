using System;
using System.Collections.Generic;
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
        public List<PlayerHand> CreatePlayerHands(Deck deck, IList<int> players, int startingWith, int handSize)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));
            var startingIndex = players.IndexOf(startingWith);
            var playersOrdered = players.Infinite().Skip(startingIndex).Take(players.Count).ToList();
            return playersOrdered.Select(p => new PlayerHand(p, deck.Skip(playersOrdered.IndexOf(p)).TakeEvery(players.Count).Take(handSize).ToList())).ToList();
        }
    }
}
