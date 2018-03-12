// Learn more about F# at http://fsharp.org

open System
open Skunked
open System.Collections.Generic
open Skunked.Rules
open Microsoft.FSharp.Collections
open Skunked.PlayingCards
open Combinatorics.Collections;


let GenerateHands = 
    let deck = new Deck()
    let combinations = new Combinations<Card>(new System.Collections.Generic.List<Card>(deck), 4)
    seq{for combo in combinations -> combo}


let ToKey(turn:Boolean, hand: list<Card>, pile: list<Card>)  =
    let key = (if turn then 0 else 1).ToString()
    let x = hand |> List.fold (fun acc elem -> acc + elem.Rank.ToString()) ""
    let y= key + x

let AreEqual(source1: Set<_>, source2: Set<_>) = 
            if source1.Count <> source2.Count then
                false  
            else
                source1 |> Seq.mapi(fun index x -> (x, index)) |> Seq.forall(fun item -> (fst item).Equals(source2 |> Seq.item(snd item)))


[<EntryPoint>]
let main argv =
    let hands = GenerateHands |> Seq.toList



    3