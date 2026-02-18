namespace Skunked.ConsoleApp;

public static class DrawCard
{
    public static Card Draw(List<Card> deck)
    {
        var c = deck[0];
        deck.RemoveAt(0);
        return c;
    }

}