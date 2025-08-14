namespace Skunked;

/// <summary>
/// The state of the Round.
/// </summary>
public sealed class RoundState
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
    public List<PlayerHand> DealtCards { get; set; } = new();

    /// <summary>
    /// Gets or sets the set of cards in each players hand after thrown to the crib.
    /// </summary>
    public List<PlayerHand> Hands { get; set; } = new();

    /// <summary>
    /// Gets or sets the set of Play items.
    /// </summary>
    public List<List<PlayItem>> ThePlay { get; set; } = new();

    /// <summary>
    /// Gets or sets the condition if the throwing of cards is complete.
    /// </summary>
    public bool ThrowCardsComplete { get; set; }

    /// <summary>
    /// Gets or sets the condition if the playing of cards is complete.
    /// </summary>
    public bool PlayedCardsComplete { get; set; }

    /// <summary>
    /// Gets or sets the state of the round being complete.
    /// </summary>
    public bool Complete { get; set; }

    /// <summary>
    /// The Id of the players crib..
    /// </summary>
    public int PlayerCrib { get; set; }

    /// <summary>
    /// The cards thrown to the crib.
    /// </summary>
    public List<Card> Crib { get; set; } = new();

    /// <summary>
    /// Gets or sets the scores for each team.
    /// </summary>
    public List<PlayerScoreShow> ShowScores { get; set; } = new();

    /// <summary>
    /// Gets or sets the preround state.
    /// </summary>
    public PreRound? PreRound { get; set; }
}