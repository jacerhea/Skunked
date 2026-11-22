using Xunit;
namespace Skunked.UnitTest;
public class CompletionRegressionTests
{
    [Fact] public void CompletionEventUpdatesTimestampDuringReplay()
    {
        var state=new GameState { Rounds=[new() { Round=1 }] }; var e=new GameCompletedEvent(Guid.NewGuid(),1);
        new GameStateBuilder(state).Notify(e);
        Assert.Equal(e.Occurred,state.CompletedAt); Assert.True(state.Rounds[0].Complete);
    }
    [Fact] public void WinningHandEmitsCompletionImmediatelyAndStopsCommands()
    {
        var game = new Cribbage([1,2], new GameRules(WinningScore.Short61));
        var state = game.LiveState;
        var hand = new List<Card> { new(Rank.Five,Suit.Clubs),new(Rank.Five,Suit.Diamonds),new(Rank.Five,Suit.Hearts),new(Rank.Jack,Suit.Spades) };
        var starter = new Card(Rank.Five,Suit.Spades);
        state.Rounds.Add(new RoundState { Round=1, PlayerCrib=1, Starter=starter, ThrowCardsComplete=true, PlayedCardsComplete=true,
            Hands=[new(2,hand)], ShowScores=[new() { Player=1 },new() { Player=2 }] });
        state.TeamScores.Single(t=>t.Players.Contains(2)).Score=60;
        game.CountHand(new(2,1));
        var completion = Assert.IsType<GameCompletedEvent>(game.Stream.Last());
        Assert.Equal(completion.Occurred,state.CompletedAt);
        Assert.True(state.GetCurrentRound().Complete);
        Assert.Throws<GameFinishedException>(()=>game.CountHand(new(1,0)));
        Assert.Single(game.Stream.OfType<GameCompletedEvent>());
    }
    [Fact] public void WinningCribDoesNotStartAnotherRound()
    {
        var game = new Cribbage([1,2], new GameRules(WinningScore.Short61)); var state=game.LiveState;
        var starter=new Card(Rank.King,Suit.Spades);
        var crib=new List<Card> { new(Rank.Ace,Suit.Clubs),new(Rank.Two,Suit.Clubs),new(Rank.Three,Suit.Clubs),new(Rank.Four,Suit.Clubs) };
        state.Rounds.Add(new RoundState { Round=1, PlayerCrib=1, Starter=starter, Crib=crib, ThrowCardsComplete=true, PlayedCardsComplete=true,
            ShowScores=[new() { Player=1, HasShowed=true },new() { Player=2, HasShowed=true }] });
        state.TeamScores.Single(t=>t.Players.Contains(1)).Score=60;
        game.CountCrib(new(1,1));
        Assert.Single(state.Rounds); Assert.IsType<GameCompletedEvent>(game.Stream.Last()); Assert.NotNull(state.CompletedAt);
    }
    [Fact] public void WinningPlayEndsGameBeforePlayFinishedEvent()
    {
        var game = new Cribbage([1,2], new GameRules(WinningScore.Short61)); var state=game.LiveState;
        var ten=new Card(Rank.Ten,Suit.Clubs); var five=new Card(Rank.Five,Suit.Hearts);
        state.Rounds.Add(new RoundState { Round=1, PlayerCrib=1, ThrowCardsComplete=true,
            Hands=[new(1,[ten,new(Rank.Ace,Suit.Clubs),new(Rank.Two,Suit.Clubs),new(Rank.Three,Suit.Clubs)]),new(2,[five,new(Rank.Six,Suit.Hearts),new(Rank.Seven,Suit.Hearts),new(Rank.Eight,Suit.Hearts)])],
            ThePlay=[[new PlayItem { Card=ten, Player=1, NextPlayer=2 }]] });
        state.TeamScores.Single(t=>t.Players.Contains(2)).Score=60;
        game.PlayCard(new(2,five));
        Assert.IsType<GameCompletedEvent>(game.Stream.Last()); Assert.NotNull(state.CompletedAt);
    }
}
