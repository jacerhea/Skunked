// Learn more about F# at http://fsharp.org
open System
open System.Collections.Generic
open Combinatorics.Collections
open FSharp.Collections.ParallelSeq
open Skunked.PlayingCards
open System.IO
open Dapper
open Newtonsoft.Json
open System.Net.Http
open System.Data.SqlClient

type SerializableCard = {
Rank: Rank
Suit: Suit}

[<Literal>]
let connectionString = @"Data Source=DESKTOP-RQTPS7L;Initial Catalog=cribbage;Integrated Security=True"
let cards = new List<Card>(new Deck())
let toKey(c:Card) = (LanguagePrimitives.EnumToValue c.Rank) + (LanguagePrimitives.EnumToValue c.Suit * 13)


//let update5Cards() =    
    //let mutable insertIndex = 0

    //let saveResult(hand: seq<Card>) =     
    //            let handOrdered = hand |> Seq.map toKey |> Seq.sort |> Seq.toList
    //            let insert = sprintf @"UPDATE [dbo].[five_hands]
    //                            SET [throwcard1] = %d
    //                            WHERE [card2] = %d,
    //                                ,[card3] = %d
    //                                ,[card4] = %d
    //                                ,[card5] = %d" handOrdered.[0] handOrdered.[1] handOrdered.[2] handOrdered.[3] handOrdered.[4]
    //            use dbConnection = new SqlConnection(connectionString)
    //            dbConnection.Open();
    //            SqlMapper.Execute(dbConnection, insert) |> ignore

    //let combinations = new Combinations<Card>(cards, 5)

    //let values = combinations 
    //                |> PSeq.withDegreeOfParallelism 50
    //                |> PSeq.map(fun combo -> combo |> Seq.sortBy(fun c -> toKey c) |> Seq.toList )
    //                |> PSeq.map(fun result -> saveResult result)
    //                |> PSeq.toList
    //values


let calculate5Cards() =    
    let mutable insertIndex = 0

    let saveResult(hand: seq<Card>, toThrow: seq<Card>) =     
                let handOrdered = hand |> Seq.map toKey |> Seq.sort |> Seq.toList
                let toThrowOrdered = toThrow |> Seq.map toKey |> Seq.sort |> Seq.toList
                let insert = sprintf @"UPDATE [dbo].[five_hands]
                        SET [throwcard1] = %d
                        WHERE   [card1] =  %d
                                AND [card2] =  %d
                                AND [card3] =  %d
                                AND [card4] =  %d
                                AND [card5] =  %d AND [throwcard1] is null"  toThrowOrdered.[0] handOrdered.[0] handOrdered.[1] handOrdered.[2] handOrdered.[3] handOrdered.[4]
                use dbConnection = new SqlConnection(connectionString)
                dbConnection.Open();
                SqlMapper.Execute(dbConnection, insert) |> ignore

        

    let combinations = new Combinations<Card>(cards, 5)
    //let client = new HttpClient()
    //client.BaseAddress <- new Uri("https://skunkedaioptimisticdecision20190419091910.azurewebsites.net")

    let decision(combo: seq<Card>) =
        CardToss.maxAverage(combo) |> Seq.toList
        //let set = combo |> Seq.toList
        //let result = client.PostAsync("api/optimistic", new StringContent(JsonConvert.SerializeObject(set))).Result.Content.ReadAsStringAsync().Result
        //let cardsToThrow = JsonConvert.DeserializeObject<List<SerializableCard>>(result) |> Seq.map(fun card -> new Card(card.Rank, card.Suit)) |> Seq.toList
        //(set, cardsToThrow)

    let runQuery(combo: int list) = 
        use dbConnection = new SqlConnection(connectionString)
        dbConnection.Open();
        SqlMapper.QuerySingle<bool>(dbConnection, sprintf @"SELECT CASE WHEN NOT EXISTS (SELECT * FROM [dbo].[five_hands] WHERE [card1] = %d and [card2] = %d and [card3] = %d and [card4] = %d and [card5] = %d and [throwcard1] not null) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END" combo.[0] combo.[1] combo.[2] combo.[3] combo.[4])

    let values = combinations 
                    //|> PSeq.withDegreeOfParallelism 50
                    |> Seq.map(fun combo -> combo |> Seq.sortBy(fun c -> toKey c) |> Seq.toList )
                    //|> PSeq.filter(fun combo -> combo |> Seq.map(fun c -> toKey c) |> Seq.toList |> fun keyCombo -> runQuery keyCombo)
                    |> Seq.map(fun combo -> (combo,decision(combo)))
                    |> Seq.map(fun result -> saveResult(result |> fst |> List.toSeq, result |> snd))
                    |> Seq.toList
    values

//let calculate6Cards() =    

//    let mutable insertIndex = 0

//    let saveResult(hand: seq<Card>, toThrow: seq<Card>) =     
//            let handOrdered = hand |> Seq.map toKey |> Seq.sort |> Seq.toList
//            let toThrowOrdered = toThrow |> Seq.map toKey |> Seq.sort |> Seq.toList
//            let insert = sprintf @"INSERT INTO [dbo].[six_hands]
//                    ([card1]
//                    ,[card2]
//                    ,[card3]
//                    ,[card4]
//                    ,[card5]
//                    ,[card6]
//                    ,[throwcard1]
//                    ,[throwcard2])
//                    VALUES (%d, %d, %d, %d, %d, %d, %d, %d)" handOrdered.[0] handOrdered.[1] handOrdered.[2] handOrdered.[3] handOrdered.[4] handOrdered.[5] toThrowOrdered.[0] toThrowOrdered.[1] 
//            use dbConnection = new SqlConnection(connectionString)
//            dbConnection.Open();
//            SqlMapper.Execute(dbConnection, insert) |> ignore

        

//    let combinations = new Combinations<Card>(cards, 6)
//    let client = new HttpClient()
//    client.BaseAddress <- new Uri("https://skunkedaioptimisticdecision20190419091910.azurewebsites.net")

//    let decision(combo: seq<Card>) =
//        let set = combo |> Seq.toList
//        let result = client.PostAsync("api/optimistic", new StringContent(JsonConvert.SerializeObject(set))).Result.Content.ReadAsStringAsync().Result
//        let cardsToThrow = JsonConvert.DeserializeObject<List<SerializableCard>>(result) |> Seq.map(fun card -> new Card(card.Rank, card.Suit)) |> Seq.toList
//        (set, cardsToThrow)

//    let runQuery(combo: int list) = 
//        use dbConnection = new SqlConnection(connectionString)
//        dbConnection.Open();
//        SqlMapper.QuerySingle<bool>(dbConnection, sprintf @"SELECT CASE WHEN NOT EXISTS (SELECT * FROM [dbo].[six_hands] WHERE [card1] = %d and [card2] = %d and [card3] = %d and [card4] = %d and [card5] = %d and [card6] = %d) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END" combo.[0] combo.[1] combo.[2] combo.[3] combo.[4] combo.[5] )

//    let values = combinations 
//                    |> PSeq.withDegreeOfParallelism 50
//                    |> PSeq.map(fun combo -> combo |> Seq.sortBy(fun c -> toKey c) |> Seq.toList )
//                    |> PSeq.filter(fun combo -> combo |> Seq.map(fun c -> toKey c) |> Seq.toList |> fun keyCombo -> runQuery keyCombo)
//                    |> PSeq.map(fun combo -> (decision(combo)))
//                    |> PSeq.map(fun result -> saveResult(result |> fst, result |> snd))
//                    |> PSeq.toList
//    values



[<EntryPoint>]
let main argv =
    calculate5Cards() |> ignore

    0
    



