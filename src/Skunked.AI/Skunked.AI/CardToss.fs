namespace Skunked.AI

module CardToss =

    open Combinatorics.Collections
    open Skunked.Score
    open Skunked.Cards
    open System.Collections.Generic

    let private calculator = new ScoreCalculator();
    let deck = new Deck()
    let possibleRemaining cards = deck |> Seq.except cards
    let combinations(cards: seq<Card>) = new Combinations<Card>(new List<Card>(cards), 4)

    let getPossibleCombos(handCombinations: Combinations<Card>, possibleStarterCards: seq<Card>) =
                handCombinations 
                |> Seq.map(fun combo -> 
                            let possibleScores = possibleStarterCards 
                                                |> Seq.map(fun cutCard -> new ScoreWithCut(Cut = cutCard, Score = calculator.CountShowPoints(cutCard, combo).Points.Score)) 
                            new ComboPossibleScores(combo, possibleScores))

    // Indent all program elements within modules that are declared with an equal sign.
    let baseDecision(hand: seq<_>) = 
        let handSet = new HashSet<Card>(hand)
        getPossibleCombos(combinations(handSet), possibleRemaining(handSet))

    let maxAverage cards = 
        let highestCombo = baseDecision(cards) 
                        |> Seq.maxBy (fun combo -> combo.GetScoreSummation())
        cards |> Seq.except highestCombo.Combo

    let minAverage cards = 
        let lowestCombo = baseDecision(cards) 
                        |> Seq.minBy (fun combo -> combo.GetScoreSummation())
        cards |> Seq.except lowestCombo.Combo


    let optimisticDecision(cards: IEnumerable<_>) = 
        let highestPossibleCombo = baseDecision(cards) 
                                |> Seq.maxBy (fun combo -> combo.PossibleScores |> Seq.maxBy (fun com -> com.Score) |> fun x -> x.Score)
        cards |> Seq.except highestPossibleCombo.Combo