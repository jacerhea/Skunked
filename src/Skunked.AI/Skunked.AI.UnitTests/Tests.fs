module Tests

open System
open Xunit
open FluentAssertions
open Skunked.Cards
open Skunked.AI

[<Fact>]
let ``Max Average Decision For Six Cards and Two Throw Aways`` () =
    let hand = [
        new Card(Rank.Five, Suit.Clubs);
        new Card(Rank.Queen, Suit.Clubs);
        new Card(Rank.Jack, Suit.Clubs);
        new Card(Rank.King, Suit.Clubs);
        new Card(Rank.Nine, Suit.Hearts);
        new Card(Rank.Two, Suit.Spades);
    ]
    let result = CardToss.maxAverage hand
    Assert.Equal(2, result |> Seq.length)
    result.Should().Contain(new Card(Rank.Two, Suit.Spades), "") |> ignore
    result.Should().Contain(new Card(Rank.Nine, Suit.Hearts), "") |> ignore

[<Fact>]
let ``Min Average Decision For Six Cards and Two Throw Aways`` () =
    let hand = [
        new Card(Rank.Five, Suit.Clubs);
        new Card(Rank.Queen, Suit.Clubs);
        new Card(Rank.Jack, Suit.Clubs);
        new Card(Rank.King, Suit.Clubs);
        new Card(Rank.Nine, Suit.Hearts);
        new Card(Rank.Two, Suit.Spades);
    ]
    let result = CardToss.minAverage hand
    Assert.Equal(2, result |> Seq.length)
    result.Should().Contain(new Card(Rank.Two, Suit.Spades), "") |> ignore
    result.Should().Contain(new Card(Rank.Nine, Suit.Hearts), "") |> ignore

[<Fact>]
let ``optimisticDecision`` () =
    let hand = [
        new Card(Rank.Five, Suit.Clubs);
        new Card(Rank.Queen, Suit.Clubs);
        new Card(Rank.Jack, Suit.Clubs);
        new Card(Rank.King, Suit.Clubs);
        new Card(Rank.Nine, Suit.Hearts);
        new Card(Rank.Two, Suit.Spades);
    ]
    let result = CardToss.optimisticDecision hand
    Assert.Equal(2, result |> Seq.length)
    Assert.Contains(new Card(Rank.Two, Suit.Spades), result)
    Assert.Contains(new Card(Rank.Nine, Suit.Hearts), result)

