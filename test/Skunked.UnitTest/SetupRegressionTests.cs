using Xunit;
namespace Skunked.UnitTest;
public class SetupRegressionTests
{
    [Theory] [InlineData(0)] [InlineData(1)] [InlineData(3)] [InlineData(5)]
    public void RejectsUnsupportedCounts(int n) => Assert.Throws<ArgumentException>(() => new Cribbage(Enumerable.Range(0,n), new()));
    [Fact] public void RejectsUnknownWinningScore() => Assert.Throws<ArgumentOutOfRangeException>(() => new GameRules((WinningScore)999));
    [Fact] public void RejectsDuplicateIds() => Assert.Throws<ArgumentException>(() => new Cribbage([1,1], new()));
    [Fact] public void RejectsUnknownCutter() { var game = new Cribbage([1,2],new()); var before=game.Stream.Count(); Assert.Throws<InvalidCribbageOperationException>(()=>game.CutCard(new(3, game.State.OpeningRound.Deck[0]))); Assert.Equal(before,game.Stream.Count()); }
}
