using Skunked.Cards;

namespace Skunked.Domain.Commands;

/// <summary>
/// Command to discard cards to the crib.
/// </summary>
public class ThrowCardsCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrowCardsCommand"/> class.
    /// </summary>
    /// <param name="playerId">The id of player.</param>
    /// <param name="cribCards">Cards being discarded to crib.</param>
    public ThrowCardsCommand(int playerId, IEnumerable<Card> cribCards)
        : base(playerId)
    {
        CribCards = cribCards;
    }

    /// <summary>
    /// Gets cards being discarded to crib.
    /// </summary>
    public IEnumerable<Card> CribCards { get; }
}