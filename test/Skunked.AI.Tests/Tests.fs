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


[<Fact>]
let PeggingSkipsIllegalCard () =
    let player = AiPlayer.OptimizedPlayer(1) :> IGameRunnerPlayer
    let pile = [ Card(Rank.King, Suit.Clubs); Card(Rank.Queen, Suit.Clubs); Card(Rank.Eight, Suit.Clubs) ]
    let illegal = Card(Rank.Ten, Suit.Hearts)
    let legal = Card(Rank.Three, Suit.Hearts)
    Assert.Equal(legal, player.DetermineCardsToPlay(GameRules(), pile, [ illegal; legal ]))

[<Fact>]
let PeggingPrefersImmediateScore () =
    let player = AiPlayer.OptimizedPlayer(1) :> IGameRunnerPlayer
    let four = Card(Rank.Four, Suit.Hearts)
    let five = Card(Rank.Five, Suit.Hearts)
    Assert.Equal(five, player.DetermineCardsToPlay(GameRules(), [ Card(Rank.Ten, Suit.Clubs) ], [ four; five ]))

[<Fact>]
let PeggingTiesAndNoLegalCard () =
    let player = AiPlayer.OptimizedPlayer(1) :> IGameRunnerPlayer
    let first = Card(Rank.Two, Suit.Clubs)
    let second = Card(Rank.Three, Suit.Hearts)
    Assert.Equal(first, player.DetermineCardsToPlay(GameRules(), [], [ first; second ]))
    let pile = [ Card(Rank.King, Suit.Clubs); Card(Rank.Queen, Suit.Clubs); Card(Rank.Ten, Suit.Clubs) ]
    Assert.Throws<InvalidOperationException>(fun () -> player.DetermineCardsToPlay(GameRules(), pile, [ first ]) |> ignore) |> ignore

[<Fact>]
let DiscardHandlesFiveCardsAndSingleEnumeration () =
    let mutable reads = 0
    let hand = seq {
        reads <- reads + 1
        if reads > 1 then failwith "Input was enumerated twice"
        yield Card(Rank.Five, Suit.Clubs)
        yield Card(Rank.Queen, Suit.Clubs)
        yield Card(Rank.Jack, Suit.Clubs)
        yield Card(Rank.King, Suit.Clubs)
        yield Card(Rank.Nine, Suit.Hearts)
    }
    let thrown = CardToss.maxAverage hand |> Seq.toList
    Assert.Single(thrown) |> ignore
    Assert.Equal(1, reads)

[<Fact>]
let DiscardRejectsInvalidHands () =
    let duplicate = Card(Rank.Five, Suit.Clubs)
    Assert.Throws<ArgumentException>(fun () -> CardToss.maxAverage [ duplicate; duplicate; Card(Rank.Jack, Suit.Clubs); Card(Rank.King, Suit.Clubs); Card(Rank.Nine, Suit.Hearts) ] |> Seq.toList |> ignore) |> ignore
    Assert.Throws<ArgumentException>(fun () -> CardToss.maxAverage [ duplicate ] |> Seq.toList |> ignore) |> ignore

[<Theory>]
[<InlineData(2)>]
[<InlineData(4)>]
let OptimizedPlayersCompleteSeededGame (count: int) =
    let players = ResizeArray<IGameRunnerPlayer>([ for id in 1..count -> AiPlayer.OptimizedPlayer(id) :> IGameRunnerPlayer ])
    let game = GameRunner(Deck(), Func<Random>(fun () -> Random(42))).Run(players, WinningScore.Short61)
    Assert.True(game.State.IsGameFinished())
    Assert.NotNull(game.State.CompletedAt)
