namespace Skunked.AI



module AiPlayer =
    open System.Collections.Generic
    open Skunked

    type OptimizedPlayer(id) =
        let idInternal = id
        let calculator = new ScoreCalculator()

        interface IGameRunnerPlayer with
            member this.Id = idInternal

            member this.CountHand(starter: Card, hand: IEnumerable<Card>) =
                let result = calculator.CountShowPoints(starter, hand, false)
                result.Points.Score

            member this.CutCards(cardsToChoose: IEnumerable<Card>)  = cardsToChoose |> Seq.head

            member this.DetermineCardsToPlay(gameRules: GameRules, pile: IEnumerable<Card>, handLeft: IEnumerable<Card>) : Card =
                handLeft |> Seq.head

            member this.DetermineCardsToThrow(hand: IEnumerable<Card>) =
                CardToss.maxAverage hand |> ResizeArray<Card>
