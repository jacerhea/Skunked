using Skunked.Cards;

namespace Skunked.Domain.State;

/// <summary>
/// Players play.
/// </summary>
public sealed class PlayItem
{
    /// <summary>
    /// Id of the player.
    /// </summary>
    public int Player { get; set; }

    /// <summary>
    /// Card played.
    /// </summary>
    public Card Card { get; set; }

    /// <summary>
    /// The count of the cards played.
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// The id of the next player.
    /// </summary>
    public int? NextPlayer { get; set; }

    /// <summary>
    /// The new count.
    /// </summary>
    public int NewCount { get; set; }
}