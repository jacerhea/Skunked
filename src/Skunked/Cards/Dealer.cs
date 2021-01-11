using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.Utility;

namespace Skunked.Cards
{
    /// <summary>
    /// Standard dealer.  One card per pass.
    /// </summary>
    public class Dealer
    {
        //todo: make the startingWith into the dealer.  this saves a step for the caller to figure out who needs to be dealt to first.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="players"></param>
        /// <param name="startingWith"></param>
        /// <param name="handSize"></param>
        /// <returns>Set of player hands in order from dealer.</returns>
        public List<PlayerHand> Deal(Deck deck, IList<int> players, int startingWith, int handSize)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));
            var startingIndex = players.IndexOf(startingWith);
            var playersOrdered = players.Infinite().Skip(startingIndex).Take(players.Count).ToList();
            return playersOrdered.Select(p => new PlayerHand(p, deck.Skip(playersOrdered.IndexOf(p)).TakeEvery(players.Count).Take(handSize).ToList())).ToList();
        }
    }
}
