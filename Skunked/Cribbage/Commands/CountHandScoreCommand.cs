using System;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CountHandScoreCommand : CribbageCommandBase, ICommand
    {
        private const int ScorePenalty = 2;


        private readonly CountHandScoreArgs _args;

        public CountHandScoreCommand(CountHandScoreArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();

            var roundState = _args.GameState.GetCurrentRound();
            var cutCard = roundState.StartingCard;
            var playerHand = roundState.PlayerHand.First(kv => kv.Key == _args.PlayerId);

            var calculatedShowScore = _args.ScoreCalculator.CountShowScore(cutCard, playerHand.Value.Cast<Card>());

            var playerScore = _args.GameState.PlayerScores.Single(ps => ps.Player == _args.PlayerId);

            if (_args.PlayerCountedScore == calculatedShowScore.Score)
            {
                playerScore.Score += calculatedShowScore.Score;
            }
            else if (_args.PlayerCountedScore > calculatedShowScore.Score)
            {
                var score = calculatedShowScore.Score - ScorePenalty;
                if(score < 0)
                {
                    
                }
                else
                {
                    playerScore.Score += score;
                }
            }
            else
            {
                playerScore.Score += _args.PlayerCountedScore;
            }

            var playerShowScore = _args.GameState.GetCurrentRound().PlayerShowScores.Single(pss => pss.Player == _args.PlayerId);
            playerShowScore.ShowScore = calculatedShowScore.Score;
            playerShowScore.HasShowed = true;
            playerShowScore.IsDone = _args.PlayerId != roundState.PlayerCrib;
            playerShowScore.PlayerCountedShowScore = _args.PlayerCountedScore;
            EndofCommandCheck();            
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.GetCurrentRound();
            if(currentRound.IsDone || !currentRound.ThrowCardsIsDone || !currentRound.PlayCardsIsDone)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCount);
            }

            if (currentRound.PlayerShowScores.Single(pss => pss.Player == _args.PlayerId).HasShowed)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.PlayerHasAlreadyCounted);
            }

            var currentPlayer = _args.GameState.Players.NextOf(_args.GameState.Players.Single(sp => sp.Id == currentRound.PlayerCrib));
            foreach (var enumeration in Enumerable.Range(1, _args.GameState.Players.Count))
            {
                var currentPSS = currentRound.PlayerShowScores.Single(pss => pss.Player == currentPlayer.Id);

                if (currentPSS.Player == _args.PlayerId) { break; }
                if(!currentPSS.HasShowed) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);}

                currentPlayer = _args.GameState.Players.NextOf(currentPlayer);
            }
        }
    }
}
