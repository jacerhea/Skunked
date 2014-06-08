using System;
using System.Collections.Generic;
using System.Linq;
using Cribbage.Commands;
using Cribbage.Commands.Arguments;
using Cribbage.Exceptions;
using Cribbage.State;
using Cribbage.Utility;
using Skunked.PlayingCards.Value;

namespace Skunked.Commands
{
    public class PlayCardCommand : CribbageCommandBase, ICommand
    {
        private readonly PlayCardArgs _args;

        public PlayCardCommand(PlayCardArgs args) : base(args)
        {
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();
            var currentRound = _args.GameState.CurrentRound();
            var setOfPlays = currentRound.PlayersShowedCards;

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => (Card)spc.Card);

            //2. 
            var currentPlayCount = _args.ScoreCalculator.SumValues(setOfPlays.Last().Select(scs => (Card)scs.Card));
            int playCount = (currentPlayCount + _args.ScoreCalculator.SumValues(new List<Card> { new Card(_args.PlayedCard) }));
            if(playCount > 31)
            {
                setOfPlays.Add(new List<PlayerPlayItem>());
            }

            var playerCardPlayedScores = setOfPlays.Last();
            var currentRoundPlayedCards = new List<Card>(playerCardPlayedScores.Select(psc => (Card)psc.Card)) { _args.PlayedCard };
            var playScore = _args.ScoreCalculator.CountThePlay(currentRoundPlayedCards);

            var playerCardPlayedScore = new PlayerPlayItem
            {
                Card = new Card(_args.PlayedCard),
                Player = _args.PlayerID
            };
            _args.GameState.PlayerScores.Single(ps => ps.Player == _args.PlayerID).Score += playScore;

            //create new round
            setOfPlays.Last().Add(playerCardPlayedScore);
            var playsLeft = _args.GameState.CurrentRound().PlayerHand.SelectMany(kv => kv.Value).Cast<Card>().Except(playedCards);
            if (playsLeft.All(c => _args.ScoreCalculator.SumValues(setOfPlays.Last().Select(spc => (Card)spc.Card).Union(new List<Card> { c })) > _args.GameState.GameRules.PlayMaxScore))
            {
                int playCountNew = _args.ScoreCalculator.SumValues(setOfPlays.Last().Select(ppi => (Card)ppi.Card));
                if (playCountNew != _args.GameState.GameRules.PlayMaxScore)
                {
                    _args.GameState.PlayerScores.Single(ps => ps.Player == _args.PlayerID).Score += _args.ScoreCalculator.GetGoValue();                    
                }
                //todo : make this less hacky.  adding than removing round so FindNextPlayer() can work
                setOfPlays.Add(new List<PlayerPlayItem>());
            }

            var nextPlayer = FindNextPlayer();
            playerCardPlayedScore.NextPlayer = nextPlayer;
            playerCardPlayedScore.Score = playScore;


            //is playing done
            bool isDone = setOfPlays.SelectMany(c => c).Select(spc => (Card)spc.Card).Count() == _args.GameState.Players.Count * _args.GameState.GameRules.HandSize;
            currentRound.PlayCardsIsDone = isDone;
            EndofCommandCheck();
        }

        protected override void ValidateState()
        {
            var currentRound = _args.GameState.CurrentRound();
            var setOfPlays = currentRound.PlayersShowedCards;

            if (!currentRound.ThrowCardsIsDone || currentRound.PlayCardsIsDone) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForPlay); }

            var allPlayerCards = currentRound.PlayerHand.Single(kv => kv.Key == _args.PlayerID).Value.Cast<Card>().ToList();
            if (allPlayerCards.Count(card => card.Equals(_args.PlayedCard)) != 1)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => (Card)spc.Card);
            if (playedCards.Any(c => c.Equals(_args.PlayedCard))) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardHasBeenPlayed); }

            if(setOfPlays.Count == 0)//todo: what is this? || setOfPlays.Last().Count == 0)
            {
                if (currentRound.PlayerCrib != _args.PlayerID)
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
                }
                else if(currentRound.PlayersShowedCards.Count == 0)
                {
                    currentRound.PlayersShowedCards.Add(new List<PlayerPlayItem>());
                }
            }

            if (setOfPlays.Last().Count > 0 && setOfPlays.SelectMany(s => s).Last().NextPlayer != _args.PlayerID)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }

            //is the player starting new round with card sum over 31 and they have a playable card for current round?
            var currentPlayCount = _args.ScoreCalculator.SumValues(setOfPlays.Last().Select(scs => (Card)scs.Card));
            int playCount = (currentPlayCount + _args.ScoreCalculator.SumValues(new List<Card> { new Card(_args.PlayedCard) }));
            if (playCount > _args.GameState.GameRules.PlayMaxScore)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => (Card) ppi.Card);
                var playersCardsLeftToPlay = allPlayerCards.Except(playedCardsThisRound).Except(new List<Card> {_args.PlayedCard});
                if (playersCardsLeftToPlay.Any(c => _args.ScoreCalculator.SumValues(new List<Card>(playedCardsThisRound){c}) <= _args.GameState.GameRules.PlayMaxScore))
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
                }
            }
        }

        private int? FindNextPlayer()
        {
            var currentRound = _args.GameState.CurrentRound();
            var setOfPlays = currentRound.PlayersShowedCards;
            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => (Card)spc.Card);
            var playerCardPlayedScores = setOfPlays.Last();

            //if round is done
            if (playedCards.Count() == _args.GameState.Players.Count * _args.GameState.GameRules.HandSize)
            {
                return null;
            }

            //move to current player
            var currentPlayer = _args.GameState.Players.Single(sp => sp.ID == _args.PlayerID);
            var nextPlayer = _args.GameState.Players.NextOf(currentPlayer);

            //move to next player with valid move
            while (true)
            {
                var nextPlayerAvailableCardsToPlay = _args.GameState.CurrentRound().PlayerHand.Single(kv => kv.Key == nextPlayer.ID).Value.Cast<Card>().Except(playedCards);
                if(nextPlayerAvailableCardsToPlay.Count() == 0)
                {
                    nextPlayer = _args.GameState.Players.NextOf(nextPlayer);
                    continue;
                }

                var nextPlayerPlaySequence = playerCardPlayedScores.Select(s => (Card) s.Card).ToList();
                nextPlayerPlaySequence.Add(nextPlayerAvailableCardsToPlay.MinBy(c => new AceLowFaceTenCardValueStrategy().ValueOf(c)));
                var scoreTest = _args.ScoreCalculator.SumValues(nextPlayerPlaySequence);
                if (scoreTest <= _args.GameState.GameRules.PlayMaxScore)
                {
                    return nextPlayer.ID;
                }

                nextPlayer = _args.GameState.Players.NextOf(nextPlayer);
            };
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
