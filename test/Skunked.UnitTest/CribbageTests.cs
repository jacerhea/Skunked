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

        // Capture the card before cutting: Handle(CardCutEvent) removes it from the deck list,
        // so openingDeck[0] would be a different card after the call.
        var expectedCard = openingDeck[0];

        // Act
        game.CutCard(new CutCardCommand(players[0], expectedCard));

        // Assert
        // Two initial events from constructor + one CardCutEvent
        Assert.Contains(collector.Events, e => e is CardCutEvent);
        var last = collector.Events.Last();
        Assert.IsType<CardCutEvent>(last);
        var cut = (CardCutEvent)last;
        Assert.Equal(players[0], cut.PlayerId);
        Assert.Equal(expectedCard, cut.CutCard);
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

    [Fact]
    public void PlayCard_Emits_CardPlayedEvent_With_Correct_Player_And_Card()
    {
        // Arrange
        var (game, collector) = AdvanceThroughThrows();
        var round = game.State.GetCurrentRound();
        var firstPlayerId = game.State.GetNextPlayerFrom(round.PlayerCrib);
        var card = round.Hands.Single(h => h.PlayerId == firstPlayerId).Hand.First();

        // Act
        game.PlayCard(new PlayCardCommand(firstPlayerId, card));

        // Assert
        var evt = collector.Events.OfType<CardPlayedEvent>().Last();
        Assert.Equal(firstPlayerId, evt.PlayerId);
        Assert.Equal(card, evt.Played);
    }

    [Fact]
    public void CountHand_Emits_HandCountedEvent_For_First_Shower()
    {
        // Arrange
        var (game, collector) = AdvanceThroughThrows();
        CompletePlays(game);
        var round = game.State.GetCurrentRound();
        var firstShowerId = game.State.GetNextPlayerFrom(round.PlayerCrib);
        var hand = round.Hands.Single(h => h.PlayerId == firstShowerId).Hand;
        var score = new ScoreCalculator().CountShowPoints(round.Starter, hand).Points.Score;

        // Act
        game.CountHand(new CountHandCommand(firstShowerId, score));

        // Assert
        Assert.Contains(collector.Events, e => e is HandCountedEvent counted && counted.PlayerId == firstShowerId);
    }

    [Fact]
    public void CountCrib_Emits_CribCountedEvent_And_Starts_Next_Round()
    {
        // Arrange
        var (game, collector) = AdvanceThroughThrows();
        CompletePlays(game);
        CompleteHandCounts(game);
        var round = game.State.GetCurrentRound();
        var cribScore = new ScoreCalculator().CountShowPoints(round.Starter, round.Crib, true).Points.Score;

        // Act
        game.CountCrib(new CountCribCommand(round.PlayerCrib, cribScore));

        // Assert
        Assert.Contains(collector.Events, e => e is CribCountedEvent);
        Assert.True(game.State.Rounds.Count >= 2, "A new round should have started after counting the crib.");
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    /// <summary>Creates a game and advances through both opening cuts and the throw phase.</summary>
    private static (Cribbage game, TestEventCollector collector) AdvanceThroughThrows(
        IReadOnlyList<int>? players = null, GameRules? rules = null)
    {
        var (game, collector) = CreateGame(players, rules);
        var openingDeck = game.State.OpeningRound.Deck;
        var playerIds = game.State.PlayerIds.ToArray();

        game.CutCard(new CutCardCommand(playerIds[0], openingDeck[0]));
        game.CutCard(new CutCardCommand(playerIds[1], openingDeck[1]));

        var round = game.State.GetCurrentRound();
        foreach (var pid in playerIds)
        {
            var hand = round.DealtCards.Single(h => h.PlayerId == pid).Hand;
            game.ThrowCards(new ThrowCardsCommand(pid, hand.Take(2).ToList()));
        }

        return (game, collector);
    }

    /// <summary>Drives the play phase to completion using each player's first legally-playable card.</summary>
    private static void CompletePlays(Cribbage game)
    {
        var scorer = new ScoreCalculator();
        var round = game.State.GetCurrentRound();
        while (!round.PlayedCardsComplete)
        {
            var lastPlay = round.ThePlay.SelectMany(p => p).LastOrDefault();
            var playerId = lastPlay?.NextPlayer ?? game.State.GetNextPlayerFrom(round.PlayerCrib);
            var allPlayed = round.ThePlay.SelectMany(p => p).Select(p => p.Card).ToList();
            var pileCount = scorer.SumValues(round.ThePlay.Last().Select(p => p.Card));
            var available = round.Hands.Single(h => h.PlayerId == playerId).Hand.Except(allPlayed);
            // Prefer a card that fits within 31; fall back to any card (triggers Go / new play round)
            var card = available.FirstOrDefault(c => pileCount + scorer.SumValues([c]) <= GameRules.Points.MaxPlayCount)
                       ?? available.First();
            game.PlayCard(new PlayCardCommand(playerId, card));
        }
    }

    /// <summary>Counts every player's hand using the actual calculated score (dealer goes last).</summary>
    private static void CompleteHandCounts(Cribbage game)
    {
        var scorer = new ScoreCalculator();
        var round = game.State.GetCurrentRound();
        var current = game.State.GetNextPlayerFrom(round.PlayerCrib);
        for (var i = 0; i < game.State.PlayerIds.Count; i++)
        {
            var hand = round.Hands.Single(h => h.PlayerId == current).Hand;
            var score = scorer.CountShowPoints(round.Starter, hand).Points.Score;
            game.CountHand(new CountHandCommand(current, score));
            current = game.State.GetNextPlayerFrom(current);
        }
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