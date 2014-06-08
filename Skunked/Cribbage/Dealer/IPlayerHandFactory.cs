using System.Collections.Generic;
using Skunked;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Cribbage.Dealer
{
    public interface IPlayerHandFactory
    {
        Dictionary<Player, List<Card>> CreatePlayerHands(Deck deck, IList<Player> players, int handSize);
    }
}