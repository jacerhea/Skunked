namespace Skunked.PlayingCards.Order
{
    public interface IOrderStrategy
    {
        int Order(Card card);
    }
}