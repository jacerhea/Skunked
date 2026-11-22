using Xunit;
namespace Skunked.UnitTest;
public class OpeningRegressionTests
{
    [Fact] public void TiedLowestPlayersRecutAndDealerControlsDealOrder()
    {
        var game = new Cribbage([1,2,3,4],new());
        game.CutCard(new(1,new(Rank.Ace,Suit.Clubs))); game.CutCard(new(2,new(Rank.Ace,Suit.Hearts)));
        game.CutCard(new(3,new(Rank.Two,Suit.Clubs))); game.CutCard(new(4,new(Rank.Three,Suit.Clubs)));
        Assert.False(game.State.OpeningRound.Complete); Assert.Equal(new[]{1,2},game.State.OpeningRound.EligiblePlayers);
        Assert.Throws<InvalidCribbageOperationException>(()=>game.CutCard(new(3,new(Rank.Four,Suit.Clubs))));
        game.CutCard(new(1,new(Rank.Four,Suit.Clubs))); game.CutCard(new(2,new(Rank.Five,Suit.Clubs)));
        Assert.Equal(1,game.State.OpeningRound.WinningPlayerCut);
        Assert.Equal(2,game.State.GetCurrentRound().DealtCards[0].PlayerId);
    }
}
