using Skunked.Cards;

namespace Skunked.Domain.Events;

/// <summary>
/// Event when a starter card has been selected.
/// </summary>
public sealed class StarterCardSelectedEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StarterCardSelectedEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="starter">The starter card that was cut.</param>
    public StarterCardSelectedEvent(Guid gameId, int version, Card starter)
        : base(gameId, version)
    {
        Starter = starter;
    }

    /// <summary>
    /// Gets the starter card.
    /// </summary>
    public Card Starter { get; }
}