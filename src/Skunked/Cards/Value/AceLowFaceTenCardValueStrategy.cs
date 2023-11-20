namespace Skunked.Cards.Value;

/// <summary>
/// Returns value of a card.  Ace = 1, 9 = 9, face cards  = 10.
/// </summary>
public class AceLowFaceTenCardValueStrategy
{
    private static readonly Dictionary<Rank, int> ValueLookup = new ()
    {
        { Rank.Ace, 1 },
        { Rank.Two, 2 },
        { Rank.Three, 3 },
        { Rank.Four, 4 },
        { Rank.Five, 5 },
        { Rank.Six, 6 },
        { Rank.Seven, 7 },
        { Rank.Eight, 8 },
        { Rank.Nine, 9 },
        { Rank.Ten, 10 },
        { Rank.Jack, 10 },
        { Rank.Queen, 10 },
        { Rank.King, 10 }
    };

    /// <summary>
    /// Get the play value.
    /// </summary>
    /// <param name="card">The card to get the value for.</param>
    /// <returns>Value of the card for the play.</returns>
    public int GetValue(Card card)
    {
        if (card == null) throw new ArgumentNullException(nameof(card));
        return ValueLookup[card.Rank];
    }
}