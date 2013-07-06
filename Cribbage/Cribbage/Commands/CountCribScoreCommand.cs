using System;
using System.Linq;
using Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands.Arguments;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Commands
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

            var currentRound = _args.GameState.CurrentRound();
            var cutCard = currentRound.StartingCard;
            var crib = currentRound.Crib.Cast<Card>();

            var calculatedCribShowScore = _args.ScoreCalculator.CountShowScore(cutCard, crib);
            var playerScore = _args.GameState.PlayerScores.Single(ps => ps.Player == _args.PlayerID);

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


            var playerShowScore = _args.GameState.CurrentRound().PlayerShowScores.Single(pss => pss.Player == _args.PlayerID);
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
            var currentRound = _args.GameState.CurrentRound();
            if (currentRound.IsDone || !currentRound.ThrowCardsIsDone || !currentRound.PlayCardsIsDone)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForCribCount);
            }

            if (!currentRound.PlayerShowScores.Where(pss => pss.Player != _args.PlayerID).All(pss => pss.HasShowed))
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }
        }
    }
}
