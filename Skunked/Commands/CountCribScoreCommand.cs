using System;
using System.Linq;
using Skunked.Exceptions;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class CountCribScoreCommand : CribbageCommandBase, ICommand
    {
        private const int ScorePenalty = 2;
        private readonly CountCribScoreArgs _args;

        public CountCribScoreCommand(CountCribScoreArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();

            var currentRound = _args.GameState.GetCurrentRound();
            var cutCard = currentRound.Starter;
            var crib = currentRound.Crib;

            var calculatedCribShowScore = _args.ScoreCalculator.CountShowScore(cutCard, crib);

            var calculatedCribScore = calculatedCribShowScore.Score;
            //penalty for overcounting
            var applicableScore = 0;
            if (_args.PlayerCountedScore == calculatedCribScore)
            {
                applicableScore = calculatedCribScore;
            }
            else if (_args.PlayerCountedScore > calculatedCribScore)
            {
                var score = calculatedCribScore - ScorePenalty;
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
            playerShowScore.CribScore = calculatedCribScore;
            playerShowScore.HasShowedCrib = true;
            playerShowScore.Complete = true;

            currentRound.Complete = true;

            EndofCommandCheck();
            //setup next round
            CreateNextRound();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        private void CreateNextRound()
        {
            var currentRound = _args.GameState.Rounds.Single(r => r.Round == _args.Round);
            currentRound.Complete = true;

            var command = new CreateNewRoundCommand(_args.GameState, _args.Round);
            command.Execute();        
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.GetCurrentRound();
            if (currentRound.Complete || !currentRound.ThrowCardsComplete || !currentRound.PlayedCardsComplete)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCribCount);
            }

            if (!currentRound.ShowScores.Where(pss => pss.Player != _args.PlayerId).All(pss => pss.HasShowed))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }
        }
    }
}
