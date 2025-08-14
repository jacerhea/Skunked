namespace Skunked;

/// <summary>
/// 
/// </summary>
public sealed class PlayerScoreShow
{
    /// <summary>
    /// 
    /// </summary>
    public int Player { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int PlayerCountedShowScore { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int ShowScore { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasShowed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasShowedCrib { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? CribScore { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool Complete { get; set; }
}