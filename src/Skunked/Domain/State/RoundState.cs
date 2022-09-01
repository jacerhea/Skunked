using Skunked.Cards;
using Skunked.Players;

namespace Skunked.Domain.State;

/// <summary>
/// The state of the Round.
/// </summary>
public class RoundState
{
    /// <summary>
    /// Gets or sets the stater card.
    /// </summary>
    public Card Starter { get; set; }

    /// <summary>
    /// Gets or sets round number.
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// Gets or sets the set of cards initially dealt to all players.
    /// </summary>
    public List<PlayerHand> DealtCards { get; set; } = new ();

    /// <summary>
    /// Gets or sets the set of cards in each players hand after thrown to the crib.
    /// </summary>
    public List<PlayerHand> Hands { get; set; } = new ();

    public List<List<PlayItem>> ThePlay { get; set; } = new ();

    public bool ThrowCardsComplete { get; set; }

    public bool PlayedCardsComplete { get; set; }

    public bool Complete { get; set; }

    public int PlayerCrib { get; set; }

    public List<Card> Crib { get; set; } = new ();

    public List<PlayerScoreShow> ShowScores { get; set; } = new ();

    public PreRound PreRound { get; set; }
}