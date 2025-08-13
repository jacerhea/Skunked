
namespace Skunked.Domain.Events;

/// <summary>
/// Event when the game has started.
/// </summary>
public sealed class GameStartedEvent : StreamEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameStartedEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="players">The players.</param>
    /// <param name="rules">The set of rules for the game.</param>
    public GameStartedEvent(Guid gameId, int version, List<int> players, GameRules rules)
        : base(gameId, version)
    {
        Players = players;
        Rules = rules;
    }

    /// <summary>
    /// Gets the set of players.
    /// </summary>
    public List<int> Players { get; }

    /// <summary>
    /// Gets the set of rules for the game.
    /// </summary>
    public GameRules Rules { get; }
}