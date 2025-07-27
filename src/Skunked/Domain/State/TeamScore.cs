using System.Collections.Generic;

namespace Skunked.Domain.State;

/// <summary>
/// The team and their score.
/// </summary>
public class TeamScore
{
    /// <summary>
    /// Player id's on a team.
    /// </summary>
    public List<int> Players { get; set; } = [];

    /// <summary>
    /// The team's score.
    /// </summary>
    public int Score { get; set; }
}