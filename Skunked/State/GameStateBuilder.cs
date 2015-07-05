using System.Collections.Generic;
using System.Linq;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.State.Events;
using Skunked.Utility;

namespace Skunked.State
{
    public class GameStateBuilder
    {
        public GameState Build(EventStream eventStream)
        {
            var state = new GameState();

            foreach (var @event in eventStream)
            {
                if (@event.GetType() == typeof (GameStartedEvent))
                {
                    Handle((GameStartedEvent)@event, state);
                }
                else if (@event.GetType() == typeof (DeckShuffledEvent))
                {
                    Handle((DeckShuffledEvent)@event, state);
                }
            }

            return state;
        }

        private void Handle(GameStartedEvent startedEvent, GameState gameState)
        {
            var deck = new Deck().ToList();

            gameState.GameRules = startedEvent.Rules;
            gameState.IndividualScores = new List<PlayerScore>(startedEvent.Players.Select(player => new PlayerScore { Player = player, Score = 0 }));
            gameState.StartedAt = startedEvent.Occurred;
            gameState.LastUpdated = startedEvent.Occurred;
            gameState.OpeningRound = new OpeningRoundState
            {
                Deck = deck,
                Complete = false,
                CutCards = new List<PlayerIdCard>(),
                WinningPlayerCut = null
            };
            gameState.TeamScores = startedEvent.Players.Count == 2
                ? startedEvent.Players.Select(p => new TeamScore { Players = new List<int> { p } }).ToList()
                : new List<TeamScore>
                {
                    new TeamScore {Players = new List<int> {startedEvent.Players[0], startedEvent.Players[2]}},
                    new TeamScore {Players = new List<int> {startedEvent.Players[1], startedEvent.Players[3]}}
                };
            gameState.Rounds = new List<RoundState>();
        }

        private void Handle(DeckShuffledEvent deckShuffledEvent, GameState gameState)
        {
            if (!gameState.OpeningRound.Complete)
            {
                gameState.OpeningRound.Deck = deckShuffledEvent.DeckState;
            }
            else
            {
                var currentRound = gameState.GetCurrentRound();
                currentRound.PreRound = currentRound.PreRound ?? new PreRound();
                currentRound.PreRound.Deck = deckShuffledEvent.DeckState;
            }
        }
    }
}
