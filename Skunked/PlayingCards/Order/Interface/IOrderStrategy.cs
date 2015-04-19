namespace Skunked.PlayingCards
{
    public interface IOrderStrategy
    {
        int Order(Card card);
    }
}