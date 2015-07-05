using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Exceptions;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.Utility;

namespace Skunked.Commands
{
    public class PlayCardCommand : CribbageCommandBase, ICommand
    {
        private readonly PlayCardArgs _args;

        public PlayCardCommand(PlayCardArgs args)
            : base(args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            _args = args;
        }

        public void Execute()
        {
            //1.  validate
            ValidateStateBase();
            
            //2. Declare round variables
            var currentRound = GameState.GetCurrentRound();
            var setOfPlays = currentRound.ThePlay;

            //3.  Set new Round before play, if necessary
            var currentPlayRound = setOfPlays.Last();
            //int playCount = _args.ScoreCalculator.SumValues(currentPlayRound.Select(scs => scs.Card).Append(_args.PlayedCard));
            //if (playCount > GameState.Rules.PlayMaxScore)
            //{
            //    var playerPlayItems = new List<PlayerPlayItem>();
            //    setOfPlays.Add(playerPlayItems);
            //    currentPlayRound = playerPlayItems;
            //}

            //4.  Card is played
            var playerCardPlayedScore = new PlayerPlayItem
            {
                Card = new Card(_args.PlayedCard),
                Player = _args.PlayerId
            };
            currentPlayRound.Add(playerCardPlayedScore);
            var playScore = _args.ScoreCalculator.CountThePlay(currentPlayRound.Select(psc => psc.Card).ToList());
            //currentPlayerScore.Score += playScore;

            //create new round
            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            var playsLeft = GameState.GetCurrentRound().Hands.SelectMany(kv => kv.Hand).Except(playedCards, CardValueEquality.Instance);
            if (playsLeft.All(c => _args.ScoreCalculator.SumValues(currentPlayRound.Select(spc => spc.Card).Append(c)) > GameRules.PlayMaxScore))
            {
                //add Go Value.  not counted if 31 as was included with ScoreCalculation.CountThePlay
                int playCountNew = _args.ScoreCalculator.SumValues(currentPlayRound.Select(ppi => ppi.Card));
                if (playCountNew != GameRules.PlayMaxScore)
                {
                    var goValue = _args.ScoreCalculator.GoValue;
                    playScore += goValue;
                }


                //not done playing, so add new play round
                setOfPlays.Add(new List<PlayerPlayItem>());
            }

            var currentPlayerScore = GameState.IndividualScores.Single(ps => ps.Player == _args.PlayerId);
            var currentTeamScore = GameState.TeamScores.Single(ps => ps.Players.Contains(_args.PlayerId));
            playerCardPlayedScore.NextPlayer = FindNextPlayer();
            playerCardPlayedScore.Score += playScore;
            currentPlayerScore.Score += playScore;
            currentTeamScore.Score += playScore;

            //5.  Check if done with Play
            bool isDone = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).Count() == GameState.PlayerIds.Count * GameRules.HandSize;
            currentRound.PlayedCardsComplete = isDone;
            EndofCommandCheck();
        }

        protected override void ValidateState()
        {
            var currentRound = GameState.GetCurrentRound();
            var setOfPlays = currentRound.ThePlay;

            if (!currentRound.ThrowCardsComplete || currentRound.PlayedCardsComplete) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForPlay); }

            var allPlayerCards = currentRound.Hands.Single(ph => ph.Id == _args.PlayerId).Hand.ToList();
            if (allPlayerCards.Count(card => card.Equals(_args.PlayedCard)) != 1)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
            }

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            if (playedCards.Any(c => c.Equals(_args.PlayedCard))) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.CardHasBeenPlayed); }
            if (!setOfPlays.Any()) { throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidStateForPlay); }

            if (setOfPlays.Count == 1 && !setOfPlays.Last().Any())
            {
                if (GameState.GetNextPlayerFrom(currentRound.PlayerCrib) != _args.PlayerId)
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
                }
            }

            if (setOfPlays.Last().Count > 0 && setOfPlays.SelectMany(s => s).Last().NextPlayer != _args.PlayerId)
            {
                throw new InvalidCribbageOperationException(InvalidCribbageOperations.NotPlayersTurn);
            }

            //is the player starting new round with card sum over 31 and they have a playable card for current round?
            var currentPlayCount = _args.ScoreCalculator.SumValues(setOfPlays.Last().Select(scs => scs.Card));
            int playCount = (currentPlayCount + _args.ScoreCalculator.SumValues(new List<Card> { new Card(_args.PlayedCard) }));
            if (playCount > GameRules.PlayMaxScore)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => ppi.Card).ToList();
                var playersCardsLeftToPlay = allPlayerCards.Except(playedCardsThisRound, CardValueEquality.Instance).Except(new List<Card> { _args.PlayedCard }, CardValueEquality.Instance);
                if (playersCardsLeftToPlay.Any(c => _args.ScoreCalculator.SumValues(new List<Card>(playedCardsThisRound) { c }) <= GameRules.PlayMaxScore))
                {
                    throw new InvalidCribbageOperationException(InvalidCribbageOperations.InvalidCard);
                }
            }
        }

        private int? FindNextPlayer()
        {
            var roundState = GameState.GetCurrentRound();
            var currentRound = roundState;
            var setOfPlays = currentRound.ThePlay;
            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).ToList();
            var playerCardPlayedScores = setOfPlays.Last();

            //if round is done
            if (playedCards.Count() == GameState.PlayerIds.Count * GameRules.HandSize)
            {
                return null;
            }

            //move to current player
            var currentPlayer = GameState.PlayerIds.Single(sp => sp == _args.PlayerId);
            var nextPlayer = GameState.PlayerIds.NextOf(currentPlayer);

            //move to next player with valid move
            while (true)
            {
                var nextPlayerAvailableCardsToPlay = roundState.Hands.Single(ph => ph.Id == nextPlayer).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                if (!nextPlayerAvailableCardsToPlay.Any())
                {
                    nextPlayer = GameState.PlayerIds.NextOf(nextPlayer);
                    continue;
                }

                var nextPlayerPlaySequence = playerCardPlayedScores.Select(s => s.Card).ToList();
                nextPlayerPlaySequence.Add(nextPlayerAvailableCardsToPlay.MinBy(c => new AceLowFaceTenCardValueStrategy().ValueOf(c)));
                var scoreTest = _args.ScoreCalculator.SumValues(nextPlayerPlaySequence);
                if (scoreTest <= GameRules.PlayMaxScore)
                {
                    return nextPlayer;
                }

                nextPlayer = GameState.PlayerIds.NextOf(nextPlayer);
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
