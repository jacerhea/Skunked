using System.Collections.Generic;
using System.Linq;
using Cribbage.Players;
using Cribbage.PlayingCards;
using Cribbage.Utility;

namespace Cribbage.Dealer
{
    /// <summary>
    /// Standard deal.  One card per pass.
    /// </summary>
    public class StandardHandDealer : IPlayerHandFactory
    {
        public Dictionary<Player, List<Card>> CreatePlayerHands(Deck deck, IList<Player> players, int handSize)
        {
            deck.Cards.Shuffle();
            var roundHands = players.ToDictionary(p => p, p => new List<Card>(handSize));

            for (int c = 0; c < players.Count * handSize; c++)
            {
                roundHands[players[c % players.Count]].Add(deck.Cards[c]);
            }

            return roundHands;
        }
    }
}
