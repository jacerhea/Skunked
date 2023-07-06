
namespace Skunked.Cards;

/// <summary>
/// Standard 52-card deck playing card.
/// </summary>
public record Card
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    public Card()
        : this(Rank.Ace) { }


    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class with the specified Rank and Suit.
    /// </summary>
    /// <param name="rank">Set Rank.</param>
    /// <param name="suit">Set Suit.</param>
    public Card(Rank rank = Rank.Ace, Suit suit = Suit.Clubs)
    {
        Rank = rank;
        Suit = suit;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    /// <param name="card">The card to make a copy of.</param>
    public Card(Card card)
    {
        Rank = card.Rank;
        Suit = card.Suit;
    }

    /// <summary>
    /// Gets playing card's rank.
    /// </summary>
    public Rank Rank { get; init; }

    /// <summary>
    /// Gets playing card's suit.
    /// </summary>
    public Suit Suit { get; init; }
}