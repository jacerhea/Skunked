using System.Collections.Generic;
using Cribbage.Players;
using Cribbage.PlayingCards;
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