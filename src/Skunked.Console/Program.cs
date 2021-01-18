using System;
using System.Collections.Generic;
using System.Linq;
using Konsole;
using Skunked.Cards;
using Skunked.Domain;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Rules;
using Skunked.Utility;
using static System.ConsoleColor;
using static System.Enum;

namespace Skunked.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new List<int> { 1, 2 };
            var state = new GameState();
            var stateEventListener = new GameStateEventListener(state, new GameStateBuilder());
            var consoleListener = new ConsoleEventListener(state);
            var cribbage = new Cribbage(players, new GameRules(WinningScoreType.Standard121, 2), new List<IEventListener> { stateEventListener, consoleListener });

            // cut opening round
            foreach (var tuple in players.Select((playerId, index) => (playerId, index)))
            {
                cribbage.CutCard(tuple.playerId, cribbage.State.OpeningRound.Deck[tuple.index]);
            }

            var readLine = Console.ReadLine();
            var toThrow = readLine.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(input => int.Parse(input));
            var userHand = cribbage.State.GetCurrentRound().DealtCards.Single(ph => ph.PlayerId == 1).Hand;
            cribbage.ThrowCards(1, toThrow.Select(value => userHand[value - 1]));
            var aiHand = cribbage.State.GetCurrentRound().DealtCards.Single(ph => ph.PlayerId == 2);
        }
    }

    public class ConsoleEventListener : IEventListener
    {
        private readonly GameState _gameState;
        private IConsole _userWindow;
        private readonly Dictionary<int, IConsole> _windowLookup;
        private IConsole _ai;
        private IConsole _crib;

        public ConsoleEventListener(GameState gameState)
        {
            _gameState = gameState;
            var standardHeight = 6;

            _userWindow = Window.OpenBox("You: 0", 40, standardHeight, new BoxStyle()
            {
                ThickNess = LineThickNess.Single,
                Title = new Colors(White, Red)
            });

            _ai = Window.OpenBox("Computer: 0", 0, 15, 40, standardHeight, new BoxStyle
            {
                ThickNess = LineThickNess.Single,
                Title = new Colors(White, Blue)
            });

            _crib = Window.OpenBox("Crib", 50, 15, 40, standardHeight, new BoxStyle
            {
                ThickNess = LineThickNess.Single,
                Title = new Colors(White, Blue)
            });


            _windowLookup = new Dictionary<int, IConsole> { { 1, _userWindow }, { 2, _ai } };
        }



        public void Notify(StreamEvent @event)
        {
            dynamic dynamicEvent = @event;
            Handle(dynamicEvent);
        }

        private void Handle(CardCutEvent @event)
        {

        }

        private void Handle(CardPlayedEvent @event)
        {

        }

        private void Handle(CardsThrownEvent @event)
        {
            var player = _windowLookup[@event.PlayerId];
            var hand = Aggregate(_gameState.GetCurrentRound().Crib);
            _crib.Clear();
            _crib.Write(hand);
            var playerHand = _gameState.GetCurrentRound().Hands.Single(ph => ph.PlayerId == @event.PlayerId);
            player.Clear();
            player.Write(Aggregate(playerHand.Hand));
        }

        private void Handle(CribCountedEvent @event)
        {

        }

        private void Handle(DeckShuffledEvent @event)
        {

        }

        private void Handle(GameCompletedEvent @event)
        {

        }

        private void Handle(GameStartedEvent @event)
        {
        }

        private void Handle(HandCountedEvent @event)
        {

        }

        private void Handle(HandsDealtEvent @event)
        {
            var hand = Aggregate(@event.Hands.Single(ph => ph.PlayerId == 1).Hand);
            var aihand = Aggregate(@event.Hands.Single(ph => ph.PlayerId == 2).Hand);
            _userWindow.Write(hand);
            _ai.Write(aihand);
        }

        private void Handle(PlayFinishedEvent @event)
        {

        }

        private void Handle(PlayStartedEvent @event)
        {

        }

        private void Handle(RoundStartedEvent @event)
        {

        }

        private void Handle(StarterCardSelectedEvent @event)
        {

        }

        private static string Aggregate(IEnumerable<Card> cards) =>
            cards
                .OrderBy(card => card.Rank)
                .ThenBy(card => card.Suit)
                .Aggregate("", (result, card) => result + " " + GetString(card));

        private static string GetString(Card card)
        {

            var rank = card.Rank < Rank.Jack ? ((int)card.Rank).ToString() : GetName(card.Rank).Substring(0, 1);
            var suitLookup = new Dictionary<Suit, string> { { Suit.Clubs, "♣" }, { Suit.Diamonds, "♦" }, { Suit.Hearts, "♥" }, { Suit.Spades, "♠" } };

            return rank + suitLookup[card.Suit];
        }

    }
}
