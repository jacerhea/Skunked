open System
open System.Collections.Generic
open Combinatorics.Collections
open FSharp.Collections.ParallelSeq
open Skunked
open System.IO

[<EntryPoint>]
let main argv =

    let cards = new List<Card>(new Deck())
    let combinations = new Combinations<Card>(cards, 6)

    let comboToString(combination:seq<Card>) = combination |> Seq.map(fun c -> ( LanguagePrimitives.EnumToValue c.Rank) + (LanguagePrimitives.EnumToValue c.Suit * 14))
                                                |> Seq.map(fun a -> a.ToString()) 
                                                |> fun j ->  String.Join("_", j)

    let decision(combo: seq<Card>) =
        let set = combo |> Seq.toList
        let result = Skunked.AI.CardToss.maxAverage(set) |> Seq.toList
        (set, result)

    let values = combinations 
                |> PSeq.withDegreeOfParallelism 12
                |> PSeq.map(fun combo -> (decision(combo)))

    use writer = new StreamWriter("aiModeling.txt", true)

    let mutable index = 0

    for result in values do 
        index <- index + 1
        writer.WriteLine ((result |> fst|> comboToString) + " -> " + (result |> snd |> comboToString))

    0
