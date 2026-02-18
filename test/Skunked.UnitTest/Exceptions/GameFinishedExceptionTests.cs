using Skunked;
using Xunit;

namespace Skunked.UnitTest.Exceptions;

public sealed class GameFinishedExceptionTests
{
    [Fact]
    public void Constructor_Creates_Exception()
    {
        var ex = new GameFinishedException();
        Assert.NotNull(ex);
    }

    [Fact]
    public void GameFinishedException_Inherits_From_Exception()
    {
        var ex = new GameFinishedException();
        Assert.IsAssignableFrom<Exception>(ex);
    }
}
