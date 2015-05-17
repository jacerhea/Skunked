using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Commands;
using Skunked.Players;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.State;
using Skunked.State.Events;

namespace Skunked
{
    public class Cribbage
    {
        private readonly GameEventStream _eventStream = new GameEventStream();

        public Cribbage()
        {
            State = new GameState();
        }

        public GameState State { get; private set; }

        public void Start(IEnumerable<Player> players, GameRules rules)
        {
            var command = new CreateNewCribbageGameCommand(_eventStream, State, players, rules);
            command.Execute();
        }

        public void CutCard(int playerId, Card card)
        {
            var command = new CutCardCommand(new CutCardArgs(_eventStream, State, playerId, State.Rounds.Count, card));
            command.Execute();

            if (State.OpeningRound.Complete)
            {
                var newRoundCommand = new CreateNewRoundCommand(State, 0);
                newRoundCommand.Execute();
            }
        }

        public void ThrowCards(int playerId, IEnumerable<Card> cribCards)
        {
            var command = new ThrowCardsToCribCommand(new ThrowCardsToCribArgs(_eventStream, State, playerId, State.Rounds.Count, cribCards));
            command.Execute();
        }

        public void PlayCard(int playerId, Card card)
        {
            var command = new PlayCardCommand(new PlayCardArgs(_eventStream, State, playerId, State.Rounds.Count, card));
            command.Execute();
        }

        public void CountHand(int playerId, int score)
        {
            var command = new CountHandScoreCommand(new CountHandScoreArgs(_eventStream, State, playerId, State.Rounds.Count, score));
            command.Execute();
        }

        public void CountCrib(int playerId, int score)
        {
            var command = new CountCribScoreCommand(new CountCribScoreArgs(_eventStream, State, playerId, State.Rounds.Count, score));
            command.Execute();
        }
    }
}
