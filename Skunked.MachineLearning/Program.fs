// Learn more about F# at http://fsharp.org

open System
open Skunked
open System.Collections.Generic
open Skunked.AI.Play
open Skunked.Rules
open Microsoft.FSharp.Collections
open Skunked.PlayingCards
open Combinatorics.Collections;
open System.Collections.ObjectModel


//let GenerateHands = 
//    let deck = new Deck()
//    let combinations = new Combinations<Card>(new System.Collections.Generic.List<Card>(deck), 4)
//    seq{for combo in combinations -> combo}


//let ToKey(turn:Boolean, hand: seq<Card>, pile: list<Card>)  =
//    let key = (if turn then 0 else 1).ToString()
//    let x = hand |> List.fold (fun acc elem -> acc + elem.Rank.ToString()) ""
//    let y= key + x
//    y

let AreEqual(source1: Set<_>, source2: Set<_>) = 
            if source1.Count <> source2.Count then
                false  
            else
                source1 |> Seq.mapi(fun index x -> (x, index)) |> Seq.forall(fun item -> (fst item).Equals(source2 |> Seq.item(snd item)))


[<EntryPoint>]
let main argv =
    let strategy = new MaxDifferencePlayStrategy();


    //let hands = GenerateHands 
    //            |> Seq.toList 
    //            |> Seq.map(fun hand -> (hand, strategy.GetRemainingPossibilities(new Skunked.AI.Play.MaxDifferencePlayStrategy.PlayState())))
    //            |> Seq.map(fun hand -> (ToKey(true, hand |> Seq.toList, []), hand) ) 
    //            |> Seq.toList
    3