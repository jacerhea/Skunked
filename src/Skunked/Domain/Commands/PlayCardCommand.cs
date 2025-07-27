using Skunked.Cards;

namespace Skunked.Domain.Commands;

/// <summary>
/// Command to play a card.
/// </summary>
public class PlayCardCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayCardCommand"/> class.
    /// </summary>
    /// <param name="playerId">The id of the player associated with the command.</param>
    /// <param name="card">The card being played.</param>
    public PlayCardCommand(int playerId, Card card)
        : base(playerId)
    {
        Card = card;
    }

    /// <summary>
    /// Gets card being played.
    /// </summary>
    public Card Card { get; }
}