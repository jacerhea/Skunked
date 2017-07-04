using System.Collections.Generic;
using Skunked.Players;
using Skunked.PlayingCards;

namespace Skunked.Dealer
{
    public interface IPlayerHandFactory
    {
        List<PlayerIdHand> CreatePlayerHands(Deck deck, IList<int> players, int startingWith, int handSize);
    }
}