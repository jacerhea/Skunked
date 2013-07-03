using System;
using System.Collections.Generic;
using Cribbage;
using Cribbage.Rules;
using Games.Domain.MainModule.Entities.CardGames.Player;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Player
{
    public interface ICribPlayer : IPlayer, IEquatable<ICribPlayer>
    {
        /// <summary>
        /// Deal Hand and return cards that will go back in crib
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        List<Card> DealHand(IList<Card> hand);
        Card PlayShow(CribGameRules gameRules, List<Card> pile, List<Card> handLeft);
        Card ChooseCard(List<Card> deck);
        int CountHand(Card card, IEnumerable<Card> hand);
    }
}