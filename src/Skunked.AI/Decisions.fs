namespace Skunked.AI

module CardToss =

    open Combinatorics.Collections
    open Skunked
    open System.Collections.Generic

    let private calculator = new ScoreCalculator()

    let deck = new Deck()
    let possibleRemaining cards = deck |> Seq.except cards

    let combinations (cards: seq<Card>) =
        new Combinations<Card>(new List<Card>(cards), 4)

    let getPossibleCombos (handCombinations: Combinations<Card>, possibleStarterCards: Collections.seq<Card>) =
        handCombinations
        |> Seq.map (fun combo ->
            let possibleScores =
                possibleStarterCards
                |> Seq.map (fun cutCard ->
                    new ScoreWithCut(Cut = cutCard, Score = calculator.CountShowPoints(cutCard, combo).Points.Score))

            new ComboPossibleScores(combo, new List<ScoreWithCut>(possibleScores)))

    let baseDecision (hand: seq<_>) =
        let handSet = new HashSet<Card>(hand)
        getPossibleCombos (combinations (handSet), possibleRemaining (handSet))

    let maxAverage cards =
        let highestCombo = baseDecision (cards) |> Seq.maxBy (fun combo -> combo.Total)
        cards |> Seq.except highestCombo.Combo

    let minAverage cards =
        let lowestCombo = baseDecision (cards) |> Seq.minBy (fun combo -> combo.Total)
        cards |> Seq.except lowestCombo.Combo

    let optimisticDecision (cards: IEnumerable<_>) =
        let handSet = new HashSet<Card>(cards)
        let combinationResult = combinations handSet
        let possible = possibleRemaining handSet
        handSet |> Seq.head
