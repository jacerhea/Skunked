using System;
using Cribbage.PlayingCards;

namespace Skunked.PlayingCards.Value
{
    public class AceLowFaceTenCardValueStrategy : ICardValueStrategy
    {

        public int ValueOf(Card card)
        {
            if (card == null) throw new ArgumentNullException("card");
            //tried as dictionary lookup, but was slower than switch
            switch (card.Rank)
            {
                case Rank.Ace:
                    return 1;
                case Rank.Two:
                    return 2;
                case Rank.Three:
                    return 3;
                case Rank.Four:
                    return 4;
                case Rank.Five:
                    return 5;
                case Rank.Six:
                    return 6;
                case Rank.Seven:
                    return 7;
                case Rank.Eight:
                    return 8;
                case Rank.Nine:
                    return 9;
                default:
                    return 10;
            }
        }

        public int ValueOf(Card card, Func<Card, int> valueFunc)
        {
            return ValueOf(card);
        }
    }
}