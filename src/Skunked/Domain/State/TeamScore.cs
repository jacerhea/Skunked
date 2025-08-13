namespace Skunked.Domain.State;

/// <summary>
/// The team and their score.
/// </summary>
public sealed class TeamScore
{
    /// <summary>
    /// Player id's on a team.
    /// </summary>
    public List<int> Players { get; set; } = new();

    /// <summary>
    /// The team's score.
    /// </summary>
    public int Score { get; set; }
}