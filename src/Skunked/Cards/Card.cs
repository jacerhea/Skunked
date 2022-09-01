
namespace Skunked.Cards;

/// <summary>
/// Standard 52-card deck playing card.
/// </summary>
public class Card : IEquatable<Card>, IEqualityComparer<Card>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    public Card()
        : this(Rank.Ace, Suit.Clubs) { }


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
    public Rank Rank { get; }

    /// <summary>
    /// Gets playing card's suit.
    /// </summary>
    public Suit Suit { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        var card = (Card)obj;
        return card.Rank == Rank && card.Suit == Suit;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return (int)Suit ^ (int)Rank;
    }


    /// <inheritdoc/>
    public bool Equals(Card other)
    {
        if (other == null)
        {
            return false;
        }
        return other.Rank == Rank && other.Suit == Suit;
    }

    /// <inheritdoc/>
    public bool Equals(Card x, Card y)
    {
        return x.Rank == y.Rank && x.Suit == y.Suit;
    }

    /// <inheritdoc/>
    public int GetHashCode(Card obj)
    {
        return (int)obj.Suit ^ (int)obj.Rank;
    }
}