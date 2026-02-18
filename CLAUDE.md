# Skunked — Claude AI Guide

## Project Overview

**Skunked** is an advanced Cribbage library for .NET. It models a complete game of Cribbage using event sourcing, providing a reusable engine for human vs. AI play, automated game simulation, scoring, and analysis. It is published as a NuGet package.

The name "Skunked" refers to the Cribbage term for losing before reaching 91 points in a standard 121-point game.

---

## Solution Structure

```
Skunked/
├── src/
│   ├── Skunked/                  # Core library (C#) — the NuGet package
│   ├── Skunked.Console/          # Interactive console game (C#)
│   ├── Skunked.AI/               # AI player decisions (F#)
│   ├── Skunked.AI.Modeling/      # Standalone AI analysis tool (F#)
│   └── Skunked.Web/              # REST API for deck operations (F#)
└── test/
    ├── Skunked.UnitTest/              # Unit tests (C#, xUnit)
    ├── Skunked.Standard.Test.System/  # System/integration tests (C#, xUnit)
    ├── Skunked.Standard.Analytics/   # BenchmarkDotNet performance benchmarks (C#)
    └── Skunked.AI.Tests/             # AI tests (F#, xUnit)
```

**The core library (`src/Skunked/`) is the primary focus.** All other projects either consume it or test it.

---

## Technology Stack

- **Languages**: C# (core + tests), F# (AI, web, modeling)
- **Runtime**: .NET 9.0
- **Testing**: xUnit, FluentAssertions, Moq
- **Benchmarking**: BenchmarkDotNet
- **Key libs**: `Combinatorics` (card combination math), `FSharp.Collections.ParallelSeq`
- **Web**: ASP.NET Core (Skunked.Web)
- **Console UI**: Goblinfactory.Konsole

---

## Core Architecture

### Event Sourcing

All game actions produce `StreamEvent` objects appended to an `EventStream`. The `GameStateBuilder` subscribes to the stream and maintains a live `GameState` by handling each event type via pattern matching. External code can subscribe additional `IEventListener` implementations to react to events.

**Event flow**: Command → Validation → `StreamEvent` emitted → `EventStream.Add()` → all `IEventListener.Notify()` called → `GameState` updated.

Key types:
- `StreamEvent` — abstract base; carries `GameId`, `Version`, `Occurred`
- `EventStream` — ordered collection of events; dispatches to listeners
- `IEventListener` — single `Notify(StreamEvent)` method
- `GameStateBuilder : IEventListener` — reconstructs `GameState` from events

### Command Pattern

Player actions are represented as command objects:
- `CutCardCommand` — opening cut to determine first dealer
- `ThrowCardsCommand` — discard to crib
- `PlayCardCommand` — play a card to the pile
- `CountHandCommand` — player declares their hand score
- `CountCribCommand` — dealer declares the crib score

Each command is validated before any event is emitted. Validation throws `InvalidCribbageOperationException` on failure.

### `Cribbage` — Main Entry Point

`Cribbage` is the top-level game class. Consumers create an instance with players and rules, then drive the game by calling its methods:

```csharp
var cribbage = new Cribbage(playerIds, new GameRules(WinningScore.Standard121));
cribbage.CutCard(new CutCardCommand(playerId, card));
cribbage.ThrowCards(new ThrowCardsCommand(playerId, cards));
cribbage.PlayCard(new PlayCardCommand(playerId, card));
cribbage.CountHand(new CountHandCommand(playerId, score));
cribbage.CountCrib(new CountCribCommand(playerId, score));
```

`Cribbage.State` gives the current `GameState`. `Cribbage.Stream` exposes the full event history.

### `GameRunner` — Automated Play

`GameRunner` runs a complete game automatically using `IGameRunnerPlayer` implementations. Used for AI testing, simulation, and benchmarking. It drives the full game loop until `GameFinishedException` is thrown.

```csharp
var runner = new GameRunner(new Deck());
var result = runner.Run(players, WinningScore.Standard121);
```

### `IGameRunnerPlayer`

Interface for implementing any automated player:
- `DetermineCardsToThrow(hand)` — choose crib discards
- `DetermineCardsToPlay(rules, pile, handLeft)` — choose play card
- `CutCards(cardsToChoose)` — choose opening cut card
- `CountHand(starter, hand)` — calculate and return hand score

### Scoring — `ScoreCalculator`

`ScoreCalculator` computes all Cribbage scoring scenarios. It is pure/stateless and can be used independently.

Key methods:
- `CountShowPoints(starterCard, hand, isCrib)` — full show scoring (fifteens, pairs, runs, flush, nobs)
- `CountPlayPoints(pile)` — play pile scoring (fifteens, pairs, pair royal, double pair royal, runs, 31, go)
- `CountCut(card)` — nibs (2 points if cut card is a Jack)
- `FindFifteens / FindPairs / FindRuns / FindFlush / FindNobs` — individual combination finders
- `GetCombinations<T>(source)` — returns all k-combinations keyed by size

Card values use `AceLowFaceTenCardValueStrategy` (Ace = 1, face cards = 10).

### State Model

- `GameState` — top-level snapshot: player IDs, scores, rounds, opening round
- `RoundState` — per-round data: dealt hands, crib, starter card, play sequence, show scores
- `OpeningRound` — tracks the opening cut to determine first dealer
- `PlayerScore / TeamScore` — individual and team scores
- `PlayItem` — a single card played to the pile, with score delta and next player pointer

Players are identified by `int` IDs. The library supports 2-player (6 cards dealt, 4 in hand) and 4-player (5 cards dealt, 4 in hand) modes. Teams are implicit in 4-player mode (players 0+2 vs 1+3).

---

## Key Cribbage Rules Encoded

- `GameRules.Points.*` — all scoring constants (Fifteen=2, Pair=2, PairRoyal=6, DoublePairRoyal=12, Flush=4, Go=1, Nibs=2, Nobs=1, MaxPlayCount=31)
- Winning scores: Standard = 121, Short = 61
- 4-card flush scores only in hand; 5-card flush also counts in crib
- `RankComparer` orders ranks for cut-card comparison (lowest rank wins the deal)
- Deck is shuffled 3 times at start of each round after the first (via `Deck.Shuffle(3)`)

---

## Namespace Convention

All types in `src/Skunked/` use the root namespace `Skunked` (no sub-namespaces). The `global using` / implicit usings pattern is enabled so common BCL types don't need explicit imports in C# files.

---

## Testing Approach

- **Unit tests** (`Skunked.UnitTest`): test individual components — scoring, card values, validation, deck operations
- **System tests** (`Skunked.Standard.Test.System`): run full games end-to-end using `TestPlayer` (a scripted `IGameRunnerPlayer`) and `GameRunner`
- **Analytics** (`Skunked.Standard.Analytics`): BenchmarkDotNet benchmarks; run with `dotnet run -c Release` from the analytics project, not as part of normal test suite
- **AI tests** (`Skunked.AI.Tests`): F# tests for the AI decision logic

Run tests with:
```bash
dotnet test
```

Run a specific project:
```bash
dotnet test test/Skunked.UnitTest/Skunked.UnitTest.csproj
```

---

## Build & CI

- Build: `dotnet build`
- Pack (NuGet): `dotnet pack src/Skunked/Skunked.csproj`
- CI (GitHub Actions):
  - `develop` branch: build + unit tests + ReSharper code quality checks
  - `master` branch: publish to NuGet.org

The library targets multiple frameworks; verify the current target in the `.csproj` before making framework-specific API calls.

---

## Best Practices for This Codebase

1. **Never bypass validation.** All commands must go through the validation classes before emitting events. Do not emit events directly from outside `Cribbage`.

2. **Preserve event sourcing integrity.** `GameState` is derived from events — never mutate it except through `GameStateBuilder.Notify()`. If you need to add state, add a new event and handle it in `GameStateBuilder`.

3. **Add a new event for new game actions.** Follow the existing pattern: create a `XyzEvent : StreamEvent`, add a `Handle(XyzEvent, GameState)` in `GameStateBuilder`, and add a case to the `switch` in `Notify()`.

4. **Keep `ScoreCalculator` stateless.** All scoring methods take inputs and return results. Do not introduce side effects or instance state.

5. **Use `IGameRunnerPlayer` for any automated player.** This is the extension point for new AI strategies or test players. Do not special-case player types inside `GameRunner` or `Cribbage`.

6. **Player IDs are arbitrary `int` values.** The library doesn't care what the IDs are, only that they are unique within a game.

7. **`GameFinishedException` signals game over.** `GameRunner` catches it to exit the game loop. Do not swallow it elsewhere — it is the intended control-flow mechanism for game completion.

8. **Crib scoring differs from hand scoring.** A 4-card flush does not score in the crib (only 5-card flushes do). This is handled by the `isCrib` parameter in `CountShowPoints`.

9. **XML doc comments are required on all public members** of `src/Skunked/`. Documentation generation is enabled in the project, and the `.xml` file is part of the NuGet package.

10. **Keep the `Skunked` namespace flat.** All public types in the core library live directly in the `Skunked` namespace. Do not introduce sub-namespaces.

---

## Common Gotchas

- The deck is shuffled during construction of `Cribbage` and again each round. Don't shuffle externally for normal play.
- `State.GetCurrentRound()` returns the most recent `RoundState`; it will throw if no rounds have started yet (before opening cut is complete).
- `PlayerIds.NextOf(id)` is a circular extension — it wraps around. Used for turn order and crib rotation.
- The `GameCompletedEvent` type exists but end-of-game detection currently relies on `GameFinishedException`. The event is not emitted in the current implementation.
- Crib miscounting penalties (`// todo: fix`) are not fully implemented — an over-counted crib currently applies no score.
