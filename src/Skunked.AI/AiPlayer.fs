namespace Skunked.AI

module AiPlayer =
    open System
    open System.Collections.Generic
    open Skunked

    /// Keeps the hand with the highest expected show score, ignoring the crib and opponent.
    /// During pegging, chooses a legal card with the highest immediate score; ties keep hand order.
    type OptimizedPlayer(id) =
        let calculator = new ScoreCalculator()

        interface IGameRunnerPlayer with
            member _.Id = id

            member _.CountHand(starter: Card, hand: IEnumerable<Card>) =
                calculator.CountShowPoints(starter, hand, false).Points.Score

            member _.CutCards(cardsToChoose: IEnumerable<Card>) = cardsToChoose |> Seq.head

            member _.DetermineCardsToPlay(_gameRules: GameRules, pile: IEnumerable<Card>, handLeft: IEnumerable<Card>) : Card =
                if isNull (box pile) then nullArg (nameof pile)
                if isNull (box handLeft) then nullArg (nameof handLeft)
                let played = pile |> Seq.toArray
                let count = calculator.SumValues(played)
                let candidates =
                    handLeft
                    |> Seq.mapi (fun index card -> index, card)
                    |> Seq.filter (fun (_, card) -> count + calculator.SumValues([ card ]) <= GameRules.Points.MaxPlayCount)
                    |> Seq.toArray
                if candidates.Length = 0 then
                    invalidOp "No card in the hand can be played without exceeding 31."
                candidates
                |> Array.maxBy (fun (index, card) ->
                    let candidatePile = new List<Card>(played)
                    candidatePile.Add(card)
                    calculator.CountPlayPoints(candidatePile), -index)
                |> snd

            member _.DetermineCardsToThrow(hand: IEnumerable<Card>) =
                CardToss.maxAverage hand |> ResizeArray<Card>
