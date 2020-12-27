using System.Collections.Generic;
using Skunked.Cards;
using Skunked.Players;

namespace Skunked.Dealer
{
    /// <summary>
    /// Deals cards to a set of players
    /// </summary>
    public interface IDealer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="players"></param>
        /// <param name="startingWith"></param>
        /// <param name="handSize"></param>
        /// <returns></returns>
        List<PlayerHand> Deal(Deck deck, IList<int> players, int startingWith, int handSize);
    }
}