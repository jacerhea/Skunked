using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Skunked.UnitTest.Gameplay;

public sealed class CribbageTests
{
    private static (Cribbage game, TestEventCollector collector) CreateGame(IReadOnlyList<int>? players = null,
        GameRules? rules = null)
    {
        var collector = new TestEventCollector();
        var ids = players ?? [1, 2];
        var gameRules = rules ?? new GameRules();
        var game = new Cribbage(ids, gameRules, [collector]);
        return (game, collector);
    }

    [Fact]
    public void Constructor_Emits_GameStarted_Then_DeckShuffled()
    {
        // Arrange + Act
        var (_, collector) = CreateGame();

        // Assert
        Assert.True(collector.Events.Count >= 2);
        Assert.IsType<GameStartedEvent>(collector.Events[0]);
        Assert.IsType<DeckShuffledEvent>(collector.Events[1]);
    }

    [Fact]
    public void CutCard_Emits_CardCutEvent()
    {
        // Arrange
        var (game, collector) = CreateGame();
        var openingDeck = game.State.OpeningRound.Deck;
        var players = game.State.PlayerIds;

        // Act
        game.CutCard(new CutCardCommand(players[0], openingDeck[0]));

        // Assert
        // Two initial events from constructor + one CardCutEvent
        Assert.Contains(collector.Events, e => e is CardCutEvent);
        var last = collector.Events.Last();
        Assert.IsType<CardCutEvent>(last);
        var cut = (CardCutEvent)last;
        Assert.Equal(players[0], cut.PlayerId);
        Assert.Equal(openingDeck[0], cut.CutCard);
    }

    [Fact]
    public void CutCard_WhenOpeningRoundCompletes_StartsRound_Shuffles_And_Deals()
    {
        // Arrange
        var (game, collector) = CreateGame();
        var openingDeck = game.State.OpeningRound.Deck;
        var players = game.State.PlayerIds.ToArray();

        // Act
        // Both players cut. After the second cut, the opening round should complete and the game should start the round.
        game.CutCard(new CutCardCommand(players[0], openingDeck[0]));
        game.CutCard(new CutCardCommand(players[1], openingDeck[1]));

        // Assert
        // We expect at least: ... CardCutEvent, CardCutEvent, RoundStartedEvent, DeckShuffledEvent, HandsDealtEvent
        var typeNames = collector.Events.Select(e => e.GetType()).ToList();
        var lastFive = typeNames.TakeLast(5).ToArray();

        Assert.Contains(typeof(CardCutEvent), lastFive);
        Assert.Contains(typeof(RoundStartedEvent), lastFive);
        Assert.Contains(typeof(DeckShuffledEvent), lastFive);
        Assert.Contains(typeof(HandsDealtEvent), lastFive);

        // Ensure state reflects a round has started and cards were dealt
        var currentRound = game.State.GetCurrentRound();
        Assert.NotNull(currentRound);
        Assert.True(currentRound.DealtCards.Count > 0);
    }

    [Fact]
    public void ThrowCards_WhenAllPlayersHaveThrown_Emits_StarterSelected_And_PlayStarted()
    {
        // Arrange
        var (game, collector) = CreateGame();
        var openingDeck = game.State.OpeningRound.Deck;
        var players = game.State.PlayerIds.ToArray();

        // Complete opening cut -> round start + deal
        game.CutCard(new CutCardCommand(players[0], openingDeck[0]));
        game.CutCard(new CutCardCommand(players[1], openingDeck[1]));

        var currentRound = game.State.GetCurrentRound();

        // Each player throws cards to the crib. We pick the first two cards from each player's current hand.
        // The validation should enforce correct counts; this mirrors typical cribbage expectations.
        var p1Hand = currentRound.DealtCards.Single(h => h.PlayerId == players[0]).Hand.ToList();
        var p2Hand = currentRound.DealtCards.Single(h => h.PlayerId == players[1]).Hand.ToList();

        var p1CribCards = p1Hand.Take(2).ToList();
        var p2CribCards = p2Hand.Take(2).ToList();

        // Act
        game.ThrowCards(new ThrowCardsCommand(players[0], p1CribCards));
        game.ThrowCards(new ThrowCardsCommand(players[1], p2CribCards));

        // Assert
        // We expect the last two events to be StarterCardSelectedEvent and PlayStartedEvent
        var lastTwo = collector.Events.TakeLast(2).ToArray();
        Assert.IsType<StarterCardSelectedEvent>(lastTwo[0]);
        Assert.IsType<PlayStartedEvent>(lastTwo[1]);

        // Also confirm state toggled the throw complete flag
        currentRound = game.State.GetCurrentRound();
        Assert.True(currentRound.ThrowCardsComplete);
        Assert.NotEqual(default, currentRound.Starter);
    }

    private sealed class TestEventCollector : IEventListener
    {
        public List<StreamEvent> Events { get; } = new();

        public void Notify(StreamEvent @event)
        {
            Events.Add(@event);
        }
    }
}