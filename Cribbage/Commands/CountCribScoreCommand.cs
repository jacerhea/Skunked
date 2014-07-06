using System;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;

namespace Skunked.Commands
{
    public class CountCribScoreCommand : CribbageCommandBase, ICommand
    {
        private const int ScorePenalty = 2;
        private readonly CountCribScoreArgs _args;

        public CountCribScoreCommand(CountCribScoreArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();

            var currentRound = _args.GameState.GetCurrentRound();
            var cutCard = currentRound.StartingCard;
            var crib = currentRound.Crib;

            var calculatedCribShowScore = _args.ScoreCalculator.CountShowScore(cutCard, crib);
            var playerScore = _args.GameState.PlayerScores.Single(ps => ps.Player == _args.PlayerId);

            var calculatedCribScore = calculatedCribShowScore.Score;
            if (_args.PlayerCountedScore == calculatedCribScore)
            {
                playerScore.Score += calculatedCribScore;
            }
            else if (_args.PlayerCountedScore > calculatedCribScore)
            {
                var score = calculatedCribScore - ScorePenalty;
                if (score < 0)
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
            playerShowScore.CribScore = calculatedCribScore;
            playerShowScore.HasShowedCrib = true;
            playerShowScore.IsDone = true;

            currentRound.IsDone = true;

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


            currentRound.IsDone = true;

            var command = new CreateNewRoundCommand(_args.GameState, _args.Round);
            command.Execute();        
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.GetCurrentRound();
            if (currentRound.IsDone || !currentRound.ThrowCardsIsDone || !currentRound.PlayCardsIsDone)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCribCount);
            }

            if (!currentRound.PlayerShowScores.Where(pss => pss.Player != _args.PlayerId).All(pss => pss.HasShowed))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }
        }
    }
}
