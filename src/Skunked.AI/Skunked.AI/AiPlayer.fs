namespace Skunked.AI



module AiPlayer = 
    open System.Collections.Generic
    open Skunked.Cards
    open Skunked.Players
    open Skunked.Rules

    type optimizedPlayer(id) = 
        let idInternal = id

        interface  IGameRunnerPlayer with
            member this.Id with get () = idInternal
            member this.CountHand(starter: Card, hand: IEnumerable<Card>): int = 
                raise (System.NotImplementedException())
            member this.CutCards(cardsToChoose: IEnumerable<Card>): Card = 
                raise (System.NotImplementedException())
            member this.DetermineCardsToPlay(gameRules: GameRules, pile: List<Card>, handLeft: List<Card>): Card = 
                raise (System.NotImplementedException())
            member this.DetermineCardsToThrow(hand: IEnumerable<Card>) =  CardToss.maxAverage hand |> ResizeArray<Card>
            

