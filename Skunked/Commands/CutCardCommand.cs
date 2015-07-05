using System;
using System.Linq;
using Skunked.Exceptions;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CutCardCommand : CribbageCommandBase, ICommand
    {
        private readonly CutCardArgs _args;

        public CutCardCommand(CutCardArgs args)
            : base(args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            _args = args;
        }

        public void Execute()
        {
            ValidateState();
            _args.GameState.OpeningRound.CutCards.Add(new PlayerIdCard{Player = _args.PlayerId, Card = new Card(_args.CutCard)});

            bool isDone = (_args.GameState.GameRules.PlayerCount == _args.GameState.OpeningRound.CutCards.Count);
            _args.GameState.OpeningRound.Complete = isDone;

            if (isDone && _args.GameState.Rounds.Count == 0)
            {
                var winningPlayerCut = _args.GameState.OpeningRound.CutCards.MinBy(playerCard => _args.OrderStrategy.Order(playerCard.Card));
                _args.GameState.OpeningRound.WinningPlayerCut = winningPlayerCut.Player;
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void ValidateState()
        {
            if (_args.GameState.OpeningRound.CutCards.Any(kv => kv.Player == _args.PlayerId))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }

            if (_args.GameState.OpeningRound.CutCards.Any(kv => kv.Card.Equals(_args.CutCard)))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.CutCardPlayerAlreadyCut);
            }
        }
    }
}
