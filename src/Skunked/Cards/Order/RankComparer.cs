namespace Skunked.Cards.Order;

/// <summary>
/// Compare by value of the Cards rank only.
/// </summary>
public class RankComparer : IComparer<Card>
{
    /// <summary>
    /// Gets instance of <see cref="RankComparer"/> RankComparer.
    /// </summary>
    public static RankComparer Instance => new();

    /// <inheritdoc />
    public int Compare(Card? x, Card? y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        return x.Rank.CompareTo(y.Rank);
    }
}