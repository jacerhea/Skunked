namespace Skunked.AI



module AiPlayer =
    open System.Collections.Generic
    open Skunked

    type OptimizedPlayer(id) =
        let idInternal = id
        let calculator = new ScoreCalculator()

        interface IGameRunnerPlayer with
            member this.Id = idInternal

            member this.CountHand(starter: Card, hand: IEnumerable<Card>) : int =
                let result = calculator.CountShowPoints(starter, hand, false)
                result.Points.Score

            member this.CutCards(cardsToChoose: IEnumerable<Card>) : Card =
                raise (System.NotImplementedException())

            member this.DetermineCardsToPlay(gameRules: GameRules, pile: List<Card>, handLeft: List<Card>) : Card =
                raise (System.NotImplementedException())

            member this.DetermineCardsToThrow(hand: IEnumerable<Card>) =
                CardToss.maxAverage hand |> ResizeArray<Card>
