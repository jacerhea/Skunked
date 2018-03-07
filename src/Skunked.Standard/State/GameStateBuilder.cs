using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Score;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State
{
    public class GameStateBuilder
    {
        private readonly ScoreCalculator _scoreCalculator;

        public GameStateBuilder()
        {
            _scoreCalculator = new ScoreCalculator();
        }

        public GameState Build(EventStream eventStream)
        {
            var state = new GameState();

            foreach (var @event in eventStream)
            {
                if (@event.GetType() == typeof(GameStartedEvent))
                {
                    Handle((GameStartedEvent)@event, state);
                }
                else if (@event.GetType() == typeof(DeckShuffledEvent))
                {
                    Handle((DeckShuffledEvent)@event, state);
                }
                else if (@event.GetType() == typeof(CardCutEvent))
                {
                    Handle((CardCutEvent)@event, state);
                }
                else if (@event.GetType() == typeof(RoundStartedEvent))
                {
                    Handle((RoundStartedEvent)@event, state);
                }
                else if (@event.GetType() == typeof(HandsDealtEvent))
                {
                    Handle((HandsDealtEvent)@event, state);
                }
                else if (@event.GetType() == typeof(StarterCardSelectedEvent))
                {
                    Handle((StarterCardSelectedEvent)@event, state);
                }
                else if (@event.GetType() == typeof(CardsThrownEvent))
                {
                    Handle((CardsThrownEvent)@event, state);
                }
                else if (@event.GetType() == typeof(CardPlayedEvent))
                {
                    Handle((CardPlayedEvent)@event, state);
                }
                else if (@event.GetType() == typeof(HandCountedEvent))
                {
                    Handle((HandCountedEvent)@event, state);
                }
                else if (@event.GetType() == typeof(CribCountedEvent))
                {
                    Handle((CribCountedEvent)@event, state);
                }
            }

            return state;
        }

        public void Handle(GameStartedEvent startedEvent, GameState gameState)
        {
            var deck = new Deck().ToList();
            gameState.Id = startedEvent.GameId;
            gameState.GameRules = startedEvent.Rules;
            gameState.StartedAt = startedEvent.Occurred;
            gameState.LastUpdated = startedEvent.Occurred;
            gameState.OpeningRound = new OpeningRound
            {
                Deck = deck,
                Complete = false,
                CutCards = new List<PlayerIdCard>(),
                WinningPlayerCut = null
            };
            gameState.IndividualScores = new List<PlayerScore>(startedEvent.Players.Select(player => new PlayerScore { Player = player, Score = 0 }));
            gameState.PlayerIds = startedEvent.Players.ToList();
            gameState.TeamScores = startedEvent.Players.Count == 2
                ? startedEvent.Players.Select(p => new TeamScore {Players = new List<int> {p}}).ToList()
                : new List<TeamScore>
                {
                    new TeamScore {Players = new List<int> {startedEvent.Players[0], startedEvent.Players[2]}},
                    new TeamScore {Players = new List<int> {startedEvent.Players[1], startedEvent.Players[3]}}
                };
            gameState.Rounds = new List<RoundState>();
        }

        public void Handle(DeckShuffledEvent deckShuffledEvent, GameState gameState)
        {
            if (!gameState.OpeningRound.Complete)
            {
                gameState.OpeningRound.Deck = deckShuffledEvent.Deck;
            }
            else
            {
                var currentRound = gameState.GetCurrentRound();
                currentRound.PreRound = currentRound.PreRound ?? new PreRound();
                currentRound.PreRound.Deck = deckShuffledEvent.Deck;
            }
        }

        public void Handle(CardCutEvent cardCutEvent, GameState gameState)
        {
            gameState.OpeningRound.CutCards.Add(new PlayerIdCard { Player = cardCutEvent.PlayerId, Card = new Card(cardCutEvent.CutCard) });

            bool isDone = gameState.GameRules.PlayerCount == gameState.OpeningRound.CutCards.Count;
            gameState.OpeningRound.Complete = isDone;

            if (isDone && gameState.Rounds.Count == 0)
            {
                var order = new StandardOrder();

                var winningPlayerCut = gameState.OpeningRound.CutCards.MinBy(playerCard => order.Order(playerCard.Card));
                gameState.OpeningRound.WinningPlayerCut = winningPlayerCut.Player;
            }
        }

        public void Handle(RoundStartedEvent roundStartedEvent, GameState gameState)
        {
            var playerShowScores = new List<PlayerScoreShow>(gameState.PlayerIds.Select(player => new PlayerScoreShow { CribScore = null, HasShowed = false, Player = player, PlayerCountedShowScore = 0, ShowScore = 0 }));

            var currentRound = gameState.Rounds.Count == 0 ? 0 : gameState.GetCurrentRound().Round;

            int cribPlayerId;
            if (gameState.OpeningRound.Complete && gameState.Rounds.Count != 0)
            {
                cribPlayerId = gameState.PlayerIds.NextOf(gameState.PlayerIds.Single(sp => gameState.Rounds.Single(r => r.Round == currentRound).PlayerCrib == sp));
            }
            else
            {
                cribPlayerId = gameState.OpeningRound.WinningPlayerCut.Value;
            }

            var roundState = new RoundState
            {
                Crib = new List<Card>(),
                DealtCards = new List<PlayerIdHand>(),
                Complete = false,
                PlayerCrib = cribPlayerId,
                Hands = new List<PlayerIdHand>(),
                ThePlay = new List<List<PlayItem>> { new List<PlayItem>() },
                Round = currentRound + 1,
                ShowScores = playerShowScores
            };

            gameState.Rounds.Add(roundState);
        }

        public void Handle(HandsDealtEvent handsDealtEvent, GameState gameState)
        {
            var round = gameState.GetCurrentRound();
            round.DealtCards = handsDealtEvent.Hands;
        }

        public void Handle(CardsThrownEvent cardsThrownEvent, GameState gameState)
        {
            var currentRound = gameState.GetCurrentRound();

            //remove thrown cards from hand
            var playerId = cardsThrownEvent.PlayerId;
            var playerHand = currentRound.DealtCards.Single(ph => ph.Id == playerId).Hand.Except(cardsThrownEvent.Thrown, CardValueEquality.Instance);
            currentRound.Hands.Add(new PlayerIdHand(playerId, playerHand.ToList()));

            currentRound.Crib.AddRange(cardsThrownEvent.Thrown);

            var playersDoneThrowing = gameState.GetCurrentRound().Crib.Count == GameRules.HandSize;
            currentRound.ThrowCardsComplete = playersDoneThrowing;
        }

        public void Handle(StarterCardSelectedEvent starterCardSelected, GameState gameState)
        {
            var currentRound = gameState.GetCurrentRound();
            var dealerId = currentRound.PlayerCrib;
            var playerScore = gameState.IndividualScores.Single(ps => ps.Player == dealerId);
            var team = gameState.TeamScores.Single(t => t.Players.Contains(dealerId));
            var cutScore = _scoreCalculator.CountCut(starterCardSelected.Starter);
            playerScore.Score += cutScore;
            team.Score += cutScore;
            currentRound.Starter = starterCardSelected.Starter;
        }

        public void Handle(CardPlayedEvent cardPlayedEvent, GameState gameState)
        {
            //2. Declare round variables
            var currentRound = gameState.GetCurrentRound();
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
            var playerCardPlayedScore = new PlayItem
            {
                Card = new Card(cardPlayedEvent.Played),
                Player = cardPlayedEvent.PlayerId
            };
            currentPlayRound.Add(playerCardPlayedScore);
            var playScore = _scoreCalculator.CountThePlay(currentPlayRound.Select(psc => psc.Card).ToList());
            //currentPlayerScore.Score += playScore;

            //create new round
            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
            var playsLeft = gameState.GetCurrentRound().Hands.SelectMany(kv => kv.Hand).Except(playedCards, CardValueEquality.Instance);
            if (playsLeft.All(c => _scoreCalculator.SumValues(currentPlayRound.Select(spc => spc.Card).Append(c)) > GameRules.PlayMaxScore))
            {
                //add Go Value.  not counted if 31 as was included with ScoreCalculation.CountThePlay
                int playCountNew = _scoreCalculator.SumValues(currentPlayRound.Select(ppi => ppi.Card));
                playerCardPlayedScore.NewCount = playCountNew;
                if (playCountNew != GameRules.PlayMaxScore)
                {
                    var goValue = _scoreCalculator.GoValue;
                    playScore += goValue;
                }


                //not done playing, so add new play round
                setOfPlays.Add(new List<PlayItem>());
            }

            var currentPlayerScore = gameState.IndividualScores.Single(ps => ps.Player == cardPlayedEvent.PlayerId);
            var currentTeamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(cardPlayedEvent.PlayerId));
            playerCardPlayedScore.NextPlayer = FindNextPlayer(gameState, cardPlayedEvent.PlayerId);
            playerCardPlayedScore.Score += playScore;
            currentPlayerScore.Score += playScore;
            currentTeamScore.Score += playScore;

            //5.  Check if done with Play
            bool isDone = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).Count() == gameState.PlayerIds.Count * GameRules.HandSize;
            currentRound.PlayedCardsComplete = isDone;
        }

        public void Handle(HandCountedEvent cardPlayedEvent, GameState gameState)
        {
            var roundState = gameState.GetCurrentRound();
            var cutCard = roundState.Starter;
            var playerHand = roundState.Hands.First(ph => ph.Id == cardPlayedEvent.PlayerId);

            var calculatedShowScore = _scoreCalculator.CountShowScore(cutCard, playerHand.Hand);

            //penalty for overcounting
            var applicableScore = 0;
            if (cardPlayedEvent.CountedScore == calculatedShowScore.Score)
            {
                applicableScore = calculatedShowScore.Score;
            }
            else if (cardPlayedEvent.CountedScore > calculatedShowScore.Score)
            {
                //todo: 
                //var score = calculatedShowScore.Score - ScorePenalty;
                //applicableScore = score < 0 ? 0 : score;
            }
            else
            {
                applicableScore = cardPlayedEvent.CountedScore;
            }
            var playerScore = gameState.IndividualScores.Single(ps => ps.Player == cardPlayedEvent.PlayerId);
            var teamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(cardPlayedEvent.PlayerId));
            playerScore.Score += applicableScore;
            teamScore.Score += applicableScore;

            var playerShowScore = gameState.GetCurrentRound().ShowScores.Single(pss => pss.Player == cardPlayedEvent.PlayerId);
            playerShowScore.ShowScore = calculatedShowScore.Score;
            playerShowScore.HasShowed = true;
            playerShowScore.Complete = cardPlayedEvent.PlayerId != roundState.PlayerCrib;
            playerShowScore.PlayerCountedShowScore = cardPlayedEvent.CountedScore;
        }

        public void Handle(CribCountedEvent cribCountedEvent, GameState gameState)
        {
            var currentRound = gameState.GetCurrentRound();
            var cutCard = currentRound.Starter;
            var crib = currentRound.Crib;

            var calculatedCribShowScore = _scoreCalculator.CountShowScore(cutCard, crib);

            var calculatedCribScore = calculatedCribShowScore.Score;
            //penalty for overcounting
            var applicableScore = 0;
            if (cribCountedEvent.CountedScore == calculatedCribScore)
            {
                applicableScore = calculatedCribScore;
            }
            else if (cribCountedEvent.CountedScore > calculatedCribScore)
            {
                //todo: fix
                //var score = calculatedCribScore - ScorePenalty;
                //applicableScore = score < 0 ? 0 : score;
            }
            else
            {
                applicableScore = cribCountedEvent.CountedScore;
            }

            var playerScore = gameState.IndividualScores.Single(ps => ps.Player == cribCountedEvent.PlayerId);
            var teamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(cribCountedEvent.PlayerId));
            playerScore.Score += applicableScore;
            teamScore.Score += applicableScore;

            var playerShowScore = gameState.GetCurrentRound().ShowScores.Single(pss => pss.Player == cribCountedEvent.PlayerId);
            playerShowScore.CribScore = calculatedCribScore;
            playerShowScore.HasShowedCrib = true;
            playerShowScore.Complete = true;

            currentRound.Complete = true;
        }

        private bool CheckEndOfGame(GameState gameState)
        {
            if (gameState.TeamScores.Any(ts => ts.Score >= gameState.GameRules.WinningScore))
            {
                gameState.CompletedAt = DateTimeOffset.Now;
                return true;
            }
            return false;
        }

        private int? FindNextPlayer(GameState state, int playerId)
        {
            var roundState = state.GetCurrentRound();
            var currentRound = roundState;
            var setOfPlays = currentRound.ThePlay;
            var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).ToList();
            var playerCardPlayedScores = setOfPlays.Last();

            //if round is done
            if (playedCards.Count == state.PlayerIds.Count * GameRules.HandSize)
            {
                return null;
            }

            //move to current player
            var currentPlayer = state.PlayerIds.Single(sp => sp == playerId);
            var nextPlayer = state.PlayerIds.NextOf(currentPlayer);

            //move to next player with valid move
            while (true)
            {
                var nextPlayerAvailableCardsToPlay = roundState.Hands.Single(ph => ph.Id == nextPlayer).Hand.Except(playedCards, CardValueEquality.Instance).ToList();
                if (!nextPlayerAvailableCardsToPlay.Any())
                {
                    nextPlayer = state.PlayerIds.NextOf(nextPlayer);
                    continue;
                }

                var nextPlayerPlaySequence = playerCardPlayedScores.Select(s => s.Card).ToList();
                nextPlayerPlaySequence.Add(nextPlayerAvailableCardsToPlay.MinBy(c => new AceLowFaceTenCardValueStrategy().ValueOf(c)));
                var scoreTest = _scoreCalculator.SumValues(nextPlayerPlaySequence);
                if (scoreTest <= GameRules.PlayMaxScore)
                {
                    return nextPlayer;
                }

                nextPlayer = state.PlayerIds.NextOf(nextPlayer);
            }
        }
    }
}
