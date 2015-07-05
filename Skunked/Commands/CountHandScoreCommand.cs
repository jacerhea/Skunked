using System;
using System.Linq;
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
            if (args == null) throw new ArgumentNullException(nameof(args));
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();

            var roundState = _args.GameState.GetCurrentRound();
            var cutCard = roundState.Starter;
            var playerHand = roundState.Hands.First(ph => ph.Id == _args.PlayerId);

            var calculatedShowScore = _args.ScoreCalculator.CountShowScore(cutCard, playerHand.Hand);

            //penalty for overcounting
            var applicableScore = 0;
            if (_args.PlayerCountedScore == calculatedShowScore.Score)
            {
                applicableScore = calculatedShowScore.Score;
            }
            else if (_args.PlayerCountedScore > calculatedShowScore.Score)
            {
                var score = calculatedShowScore.Score - ScorePenalty;
                applicableScore = score < 0 ? 0 : score;
            }
            else
            {
                applicableScore = _args.PlayerCountedScore;
            }
            var playerScore = _args.GameState.IndividualScores.Single(ps => ps.Player == _args.PlayerId);
            var teamScore = _args.GameState.TeamScores.Single(ps => ps.Players.Contains(_args.PlayerId));
            playerScore.Score += applicableScore;
            teamScore.Score += applicableScore;


            var playerShowScore = _args.GameState.GetCurrentRound().ShowScores.Single(pss => pss.Player == _args.PlayerId);
            playerShowScore.ShowScore = calculatedShowScore.Score;
            playerShowScore.HasShowed = true;
            playerShowScore.Complete = _args.PlayerId != roundState.PlayerCrib;
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
            if(currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCount);
            }

            if (currentRound.ShowScores.Single(pss => pss.Player == _args.PlayerId).HasShowed)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.PlayerHasAlreadyCounted);
            }

            var currentPlayer = _args.GameState.PlayerIds.NextOf(_args.GameState.PlayerIds.Single(id => id == currentRound.PlayerCrib));
            foreach (var enumeration in Enumerable.Range(1, _args.GameState.PlayerIds.Count))
            {
                var playerScoreShow = currentRound.ShowScores.Single(pss => pss.Player == currentPlayer);

                if (playerScoreShow.Player == _args.PlayerId) { break; }
                if(!playerScoreShow.HasShowed) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);}

                currentPlayer = _args.GameState.PlayerIds.NextOf(currentPlayer);
            }
        }
    }
}
