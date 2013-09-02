using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Commands.Arguments;
using Cribbage.Exceptions;
using Cribbage.Utility;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands;

namespace Cribbage.Commands
{
    public class ThrowCardsToCribCommand : CribbageCommandBase, ICommand
    {
        private readonly ThrowCardsToCribArgs _args;

        public ThrowCardsToCribCommand(ThrowCardsToCribArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();
            var currentRound = _args.GameState.Rounds.MaxBy(round => round.Round);
            var playerDealtHand = currentRound.PlayerDealtCards.First(kv => kv.Key == _args.PlayerID);
            var dealtCards = playerDealtHand.Value.Select(c => (Card)c);

            //remove thrown cards from hand
            var playerHand = currentRound.PlayerDealtCards.First(kv => kv.Key == _args.PlayerID).Value.Cast<Card>().Except(_args.CardsToThrow);
            currentRound.PlayerHand.Add(new SerializableKeyValuePair<int, List<SerializableCard>> { Key = _args.PlayerID, Value = playerHand.Cast<SerializableCard>().ToList() });

            var serializableCards = _args.CardsToThrow.Select(c => new SerializableCard(c));
            currentRound.Crib.AddRange(serializableCards);

            var playersDoneThrowing = _args.GameState.CurrentRound().Crib.Count == _args.GameState.GameRules.HandSize;
            if(playersDoneThrowing)
            {
                var deck = EnumHelper.GetValues<Rank>().Cartesian(EnumHelper.GetValues<Suit>(), (rank, suit) => new SerializableCard { Rank = rank, Suit = suit }).ToList();
                var cardsNotDealt = deck.Except(currentRound.Crib).Except(currentRound.PlayerHand.SelectMany(s => s.Value)).ToList();

                var randomIndex = new Random().Next(0, cardsNotDealt.Count - 1);
                var startingCard = cardsNotDealt[randomIndex];

                var playerScore = _args.GameState.PlayerScores.First(ps => ps.Player == _args.PlayerID);
                playerScore.Score += _args.ScoreCalculator.CountCut(startingCard);
                currentRound.StartingCard = startingCard;
            }

            currentRound.ThrowCardsIsDone = playersDoneThrowing;
            EndofCommandCheck();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.Rounds.MaxBy(round => round.Round);
            var playerDealtHand = currentRound.PlayerDealtCards.First(kv => kv.Key == _args.PlayerID);
            var dealtCards = playerDealtHand.Value.Select(c => (Card)c);

            if (dealtCards.Intersect(_args.CardsToThrow).Count() != _args.CardsToThrow.Count())
            {
                //invalid request, player was not dealt these cards
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            if(currentRound.Crib.Cast<Card>().Intersect(_args.CardsToThrow).Any())
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardsHaveBeenThrown);
            }
        }
    }
}
