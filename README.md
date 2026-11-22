# Skunked

A .NET library for modeling and scoring cribbage games. The core engine supports two players or four players in fixed teams. The separate `Skunked.AI` package provides automated players.

## Install

```sh
dotnet add package Skunked
# Optional, for AI players:
dotnet add package Skunked.AI
```

The core library targets .NET 8, 9, and 10. The AI and console projects target .NET 10. The repository uses the .NET 10 SDK specified in `global.json`.

## Start a game

The following example completes an opening cut and deals the first round:

```csharp
using System;
using System.Linq;
using Skunked;

var game = new Cribbage(new[] { 1, 2 }, new GameRules(WinningScore.Standard121));

// Choose different ranks so this example completes the opening cut without a tie.
var firstCut = game.State.OpeningRound.Deck.First(card => card.Rank == Rank.Ace);
game.CutCard(new CutCardCommand(1, firstCut));
var secondCut = game.State.OpeningRound.Deck.First(card => card.Rank == Rank.Two);
game.CutCard(new CutCardCommand(2, secondCut));

var round = game.State.GetCurrentRound();
Console.WriteLine($"Dealer: {round.PlayerCrib}");
Console.WriteLine($"Cards dealt to each player: {round.DealtCards[0].Hand.Count}");
```

The same code is checked into [`examples/GettingStarted`](examples/GettingStarted) and can be run from the repository with:

```sh
dotnet run --project examples/GettingStarted/GettingStarted.csproj
```

Send `ThrowCardsCommand`, `PlayCardCommand`, `CountHandCommand`, and `CountCribCommand` to advance the game. Use the dealer and hands in the current round to determine the next action. If opening cuts tie for the lowest rank, only the tied players cut again.

`game.State` returns a detached snapshot. Read it again after each command; modifying it does not change the game. A round's `Starter` is `null` until the starter has been selected. `game.Stream` exposes the event history, and `GameStateBuilder.Replay(game.Stream)` reconstructs a state from an ordered history. When a team reaches the winning score, the engine emits `GameCompletedEvent` and records `CompletedAt`; later commands throw `GameFinishedException`.

`GameRunner` runs a complete game using `IGameRunnerPlayer` implementations. `ScoreCalculator.CountShowPoints` can also score a hand independently; pass `isCrib: true` when scoring a crib.

## Build and test

```sh
dotnet build Skunked.sln -c Release
dotnet test Skunked.sln -c Release
```

The core unit tests run on .NET 8, 9, and 10. The system and AI tests run on .NET 10. When changing core framework support, update the target list, CI matrix, and this document together. See [CONTRIBUTING.md](CONTRIBUTING.md) for API and documentation conventions.

## Packaging

```sh
dotnet pack src/Skunked/Skunked.csproj -c Release
dotnet pack src/Skunked.AI/Skunked.AI.fsproj -c Release
pwsh ./scripts/Verify-Packages.ps1
```

The verification script checks package contents, XML documentation, symbols, dependencies, and a consumer using both generated packages. GitHub Actions runs the solution checks, framework matrix, and package verification for pull requests and pushes to `main` and `develop`.
