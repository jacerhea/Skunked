namespace Skunked.PlayingCards.Value
{
    public interface ICardValueStrategy
    {
        int ValueOf(Card card);
    }
}
