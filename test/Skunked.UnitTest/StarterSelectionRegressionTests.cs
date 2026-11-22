using Xunit;
namespace Skunked.UnitTest;
public class StarterSelectionRegressionTests
{
    [Fact] public void LastEligibleCardCanBeSelected()
    {
        Assert.Equal(4, Cribbage.ChooseStarterIndex(new LastRandom(), 5));
    }
    private sealed class LastRandom : Random { public override int Next(int maxValue) => maxValue - 1; }
}
