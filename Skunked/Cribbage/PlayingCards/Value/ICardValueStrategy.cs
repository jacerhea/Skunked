using Skunked;

namespace Cribbage.PlayingCards
{
    public interface ICardValueStrategy
    {
        int ValueOf(Card card);
    }
}
