namespace Skunked.AI

/// Discard decisions based on the expected show score of the retained hand.
module CardToss =

    open System
    open System.Collections.Generic
    open Combinatorics.Collections
    open Skunked

    let private calculator = new ScoreCalculator()

    let deck = new Deck()
    let possibleRemaining cards = deck |> Seq.except cards

    let combinations (cards: seq<Card>) =
        new Combinations<Card>(new List<Card>(cards), 4)

    let getPossibleCombos (handCombinations: Combinations<Card>, possibleStarterCards: seq<Card>) =
        let starters = possibleStarterCards |> Seq.toArray
        handCombinations
        |> Seq.map (fun combo ->
            let possibleScores =
                starters
                |> Seq.map (fun cutCard ->
                    new ScoreWithCut(Cut = cutCard, Score = calculator.CountShowPoints(cutCard, combo).Points.Score))
            new ComboPossibleScores(combo, new List<ScoreWithCut>(possibleScores)))

    /// Scores each four-card keep against every starter not in the original hand.
    /// Crib ownership, the opponent's cards, and pegging are not modeled.
    let baseDecision (hand: seq<Card>) =
        if isNull (box hand) then nullArg (nameof hand)
        let cards = hand |> Seq.toList
        if cards.Length < 4 || cards.Length > 6 then
            invalidArg (nameof hand) "The hand must contain four, five, or six distinct cards."
        let validCards = HashSet<Card>(deck)
        if (cards |> Seq.distinct |> Seq.length) <> cards.Length ||
           (cards |> List.exists (fun card -> isNull (box card) || not (validCards.Contains(card)))) then
            invalidArg (nameof hand) "The hand must contain distinct standard playing cards."
        getPossibleCombos (combinations cards, possibleRemaining cards)

    /// Discards the cards outside the keep with the highest mean show score.
    /// Ties use the first combination in input order.
    let maxAverage (cards: seq<Card>) : seq<Card> =
        if isNull (box cards) then nullArg (nameof cards)
        let hand = cards |> Seq.toList
        let _, highestCombo = baseDecision hand |> Seq.mapi (fun index combo -> index, combo) |> Seq.maxBy (fun (index, combo) -> combo.Total, -index)
        hand |> Seq.except highestCombo.Combo

    /// Discards the cards outside the keep with the lowest mean show score.
    /// Intended for comparisons; ties use the first combination in input order.
    let minAverage (cards: seq<Card>) : seq<Card> =
        if isNull (box cards) then nullArg (nameof cards)
        let hand = cards |> Seq.toList
        let _, lowestCombo = baseDecision hand |> Seq.mapi (fun index combo -> index, combo) |> Seq.minBy (fun (index, combo) -> combo.Total, index)
        hand |> Seq.except lowestCombo.Combo
