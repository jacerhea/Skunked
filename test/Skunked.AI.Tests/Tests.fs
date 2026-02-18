module Tests

open System
open Xunit
open Skunked
open Skunked.AI

[<Fact>]
let ``Max Average Decision For Six Cards and Two Throw Aways`` () =
    let hand =
        [ new Card(Rank.Five, Suit.Clubs)
          new Card(Rank.Queen, Suit.Clubs)
          new Card(Rank.Jack, Suit.Clubs)
          new Card(Rank.King, Suit.Clubs)
          new Card(Rank.Nine, Suit.Hearts)
          new Card(Rank.Two, Suit.Spades) ]

    let result = CardToss.maxAverage hand
    Assert.Equal(2, result |> Seq.length)
    Assert.Contains(new Card(Rank.Two, Suit.Spades), result)
    Assert.Contains(new Card(Rank.Nine, Suit.Hearts), result)
