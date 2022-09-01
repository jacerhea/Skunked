using Skunked.Cards;

namespace Skunked.Domain.Commands;

/// <summary>
/// Command to cut the deck.
/// </summary>
public class CutCardCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CutCardCommand"/> class.
    /// </summary>
    /// <param name="playerId">The id of player.</param>
    /// <param name="cutCard">Card that was cut.</param>
    public CutCardCommand(int playerId, Card cutCard)
        : base(playerId)
    {
        CutCard = cutCard;
    }

    /// <summary>
    /// Gets card that was cut.
    /// </summary>
    public Card CutCard { get; }
}