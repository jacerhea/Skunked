﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands.Arguments;
using Skunked.Exceptions;
using Skunked.PlayingCards.Value;
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
            if (args == null) throw new ArgumentNullException("args");
            _args = args;
        }

        public void Execute()
        {
            ValidateStateBase();
            var currentRound = GameState.GetCurrentRound();
            var setOfPlays = currentRound.ThePlay;

            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);

            //2. 
            var currentPlayRound = setOfPlays.Last();
            var currentPlayCount = _args.ScoreCalculator.SumValues(currentPlayRound.Select(scs => scs.Card));
            int playCount = (currentPlayCount + _args.ScoreCalculator.SumValues(new List<Card> { new Card(_args.PlayedCard) }));
            if (playCount > 31)
            {
                setOfPlays.Add(new List<PlayerPlayItem>());
            }

            var playerCardPlayedScores = currentPlayRound;
            var currentRoundPlayedCards = new List<Card>(playerCardPlayedScores.Select(psc => psc.Card)) { _args.PlayedCard };
            var playScore = _args.ScoreCalculator.CountThePlay(currentRoundPlayedCards);

            var playerCardPlayedScore = new PlayerPlayItem
            {
                Card = new Card(_args.PlayedCard),
                Player = _args.PlayerId
            };
            var currentPlayerScore = GameState.Scores.Single(ps => ps.Player == _args.PlayerId);
            currentPlayerScore.Score += playScore;

            //create new round
            currentPlayRound.Add(playerCardPlayedScore);
            var playsLeft = GameState.GetCurrentRound().Hands.SelectMany(kv => kv.Hand).Except(playedCards, CardValueEquality.Instance);
            if (playsLeft.All(c => _args.ScoreCalculator.SumValues(currentPlayRound.Select(spc => spc.Card).Union(new List<Card> { c }, CardValueEquality.Instance)) > GameState.Rules.PlayMaxScore))
            {
                int playCountNew = _args.ScoreCalculator.SumValues(currentPlayRound.Select(ppi => ppi.Card));
                if (playCountNew != GameState.Rules.PlayMaxScore)
                {
                    var goValue = _args.ScoreCalculator.GoValue;
                    currentPlayerScore.Score += _args.ScoreCalculator.GoValue;
                    playerCardPlayedScore.Score += goValue;
                }
                setOfPlays.Add(new List<PlayerPlayItem>());
            }

            var nextPlayer = FindNextPlayer();
            playerCardPlayedScore.NextPlayer = nextPlayer;
            playerCardPlayedScore.Score += playScore;


            //is playing done
            bool isDone = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).Count() == GameState.Players.Count * GameState.Rules.HandSize;
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
                if (GameState.GetNextPlayerFrom(currentRound.PlayerCrib).Id != _args.PlayerId)
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
            if (playCount > GameState.Rules.PlayMaxScore)
            {
                var playedCardsThisRound = setOfPlays.Last().Select(ppi => ppi.Card).ToList();
                var playersCardsLeftToPlay = allPlayerCards.Except(playedCardsThisRound, CardValueEquality.Instance).Except(new List<Card> { _args.PlayedCard }, CardValueEquality.Instance);
                if (playersCardsLeftToPlay.Any(c => _args.ScoreCalculator.SumValues(new List<Card>(playedCardsThisRound) { c }) <= GameState.Rules.PlayMaxScore))
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
            if (playedCards.Count() == GameState.Players.Count * GameState.Rules.HandSize)
            {
                return null;
            }

            //move to current player
            var currentPlayer = GameState.Players.Single(sp => sp.Id == _args.PlayerId);
            var nextPlayer = GameState.Players.NextOf(currentPlayer);

            //move to next player with valid move
            while (true)
            {
                var nextPlayerAvailableCardsToPlay = roundState.Hands.Single(ph => ph.Id == nextPlayer.Id).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                if (!nextPlayerAvailableCardsToPlay.Any())
                {
                    nextPlayer = GameState.Players.NextOf(nextPlayer);
                    continue;
                }

                var nextPlayerPlaySequence = playerCardPlayedScores.Select(s => s.Card).ToList();
                nextPlayerPlaySequence.Add(nextPlayerAvailableCardsToPlay.MinBy(c => new AceLowFaceTenCardValueStrategy().ValueOf(c)));
                var scoreTest = _args.ScoreCalculator.SumValues(nextPlayerPlaySequence);
                if (scoreTest <= GameState.Rules.PlayMaxScore)
                {
                    return nextPlayer.Id;
                }

                nextPlayer = GameState.Players.NextOf(nextPlayer);
            }
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
