using Xunit;
namespace Skunked.UnitTest;
public class CommandRegressionTests
{
    [Fact] public void CommandsBeforeOpeningRoundFailAsDomainErrors()
    {
        var game=new Cribbage([1,2],new()); var before=game.Stream.Count();
        Assert.Throws<InvalidCribbageOperationException>(()=>game.ThrowCards(new(1, [])));
        Assert.Throws<InvalidCribbageOperationException>(()=>game.CountHand(new(1, 0)));
        Assert.Throws<InvalidCribbageOperationException>(()=>game.CountCrib(new(1, 0)));
        Assert.Equal(before,game.Stream.Count());
    }
}
