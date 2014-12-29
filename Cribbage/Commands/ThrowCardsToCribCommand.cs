using System;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class ThrowCardsToCribCommand : CribbageCommandBase, ICommand
    {
        private readonly ThrowCardsToCribArgs _args;

        public ThrowCardsToCribCommand(ThrowCardsToCribArgs args)
            : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();
            var currentRound = _args.GameState.Rounds.MaxBy(round => round.Round);

            //remove thrown cards from hand
            var playerHand = currentRound.DealtCards.First(ph => ph.Id == _args.PlayerId).Hand.Except(_args.CardsToThrow, CardValueEquality.Instance);
            currentRound.Hands.Add(new PlayerIdHand(_args.PlayerId, playerHand.ToList()));

            var serializableCards = _args.CardsToThrow.Select(c => new Card(c));
            currentRound.Crib.AddRange(serializableCards);

            var playersDoneThrowing = _args.GameState.GetCurrentRound().Crib.Count == _args.GameState.Rules.HandSize;
            if (playersDoneThrowing)
            {
                var deck = new Deck();
                var cardsNotDealt = deck.Except(currentRound.Crib).Except(currentRound.Hands.SelectMany(s => s.Hand), CardValueEquality.Instance).ToList();

                var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsNotDealt.Count - 1);
                var startingCard = cardsNotDealt[randomIndex];

                var playerScore = _args.GameState.IndividualScores.First(ps => ps.Player == _args.PlayerId);
                playerScore.Score += _args.ScoreCalculator.CountCut(startingCard);
                currentRound.Starter = startingCard;
            }

            currentRound.ThrowCardsComplete = playersDoneThrowing;
            EndofCommandCheck();
        }

        public void Undo()
        {
            UndoBase();
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.GetCurrentRound();
            var playerDealtHand = currentRound.DealtCards.First(playerHand => playerHand.Id == _args.PlayerId);
            var dealtCards = playerDealtHand.Hand;

            if (dealtCards.Intersect(_args.CardsToThrow).Count() != _args.CardsToThrow.Count())
            {
                //invalid request, player was not dealt these cards
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            if (currentRound.Crib.Intersect(_args.CardsToThrow).Any())
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardsHaveBeenThrown);
            }
        }
    }
}
