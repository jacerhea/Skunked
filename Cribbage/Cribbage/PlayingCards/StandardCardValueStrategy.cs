using System;

namespace Cribbage.PlayingCards
{
    class StandardCardValueStrategy : ICardValueStrategy
    {
        public int ValueOf(Card card)
        {
            if (card == null) throw new ArgumentNullException("card");
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
                case Rank.Ten:
                    return 10;
                case Rank.Jack:
                    return 11;
                case Rank.Queen:
                    return 12;
                case Rank.King:
                    return 13;
                default:
                    throw new ArgumentException("Card value could not be found.");
            }
        }
    }
}