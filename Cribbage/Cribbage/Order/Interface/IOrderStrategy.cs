namespace Cribbage.Order.Interface
{
    public interface IOrderStrategy
    {
        int Order(Card card);
    }
}