using Skunked;
using Xunit;

namespace Skunked.UnitTest.Rules;

public sealed class GameRulesTests
{
    [Fact]
    public void Default_Constructor_WinningScore_Is_121()
    {
        var rules = new GameRules();
        Assert.Equal(121, rules.WinningScore);
    }

    [Fact]
    public void Standard121_WinningScore_Is_121()
    {
        var rules = new GameRules(WinningScore.Standard121);
        Assert.Equal(121, rules.WinningScore);
    }

    [Fact]
    public void Short61_WinningScore_Is_61()
    {
        var rules = new GameRules(WinningScore.Short61);
        Assert.Equal(61, rules.WinningScore);
    }

    [Fact]
    public void HandSize_Is_4()
    {
        Assert.Equal(4, GameRules.HandSize);
    }

    [Theory]
    [InlineData(2, 6)]
    [InlineData(3, 5)]
    [InlineData(4, 5)]
    public void GetDealSize_Returns_Correct_Count_For_Player_Count(int playerCount, int expectedDealSize)
    {
        var rules = new GameRules();
        Assert.Equal(expectedDealSize, rules.GetDealSize(playerCount));
    }

    [Fact] public void Points_Go_Is_1()              => Assert.Equal(1,  GameRules.Points.Go);
    [Fact] public void Points_MaxPlayCount_Is_31()   => Assert.Equal(31, GameRules.Points.MaxPlayCount);
    [Fact] public void Points_Fifteen_Is_2()         => Assert.Equal(2,  GameRules.Points.Fifteen);
    [Fact] public void Points_Nibs_Is_2()            => Assert.Equal(2,  GameRules.Points.Nibs);
    [Fact] public void Points_Nobs_Is_1()            => Assert.Equal(1,  GameRules.Points.Nobs);
    [Fact] public void Points_Pair_Is_2()            => Assert.Equal(2,  GameRules.Points.Pair);
    [Fact] public void Points_PairRoyal_Is_6()       => Assert.Equal(6,  GameRules.Points.PairRoyal);
    [Fact] public void Points_DoublePairRoyal_Is_12()=> Assert.Equal(12, GameRules.Points.DoublePairRoyal);
    [Fact] public void Points_Flush_Is_4()           => Assert.Equal(4,  GameRules.Points.Flush);
}
