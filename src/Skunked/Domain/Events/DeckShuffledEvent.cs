using Skunked.Cards;

namespace Skunked.Domain.Events;

/// <summary>
/// Event when the deck has been shuffled.
/// </summary>
public class DeckShuffledEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeckShuffledEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="deck">The shuffled deck.</param>
    public DeckShuffledEvent(Guid gameId, int version, List<Card> deck)
        : base(gameId, version)
    {
        Deck = deck;
    }

    /// <summary>
    /// Gets the new state of the deck after the shuffle.
    /// </summary>
    public List<Card> Deck { get; }
}