# Skunked project guide

Skunked is a cribbage engine and scoring library for .NET. The core library is in `src/Skunked`, the F# AI in `src/Skunked.AI`, the console sample in `src/Skunked.Console`, and AI modeling in `src/Skunked.AI.Modeling`. Tests and benchmarks are under `test/`. The previous web project has been removed.

## Build and tests

Use the .NET 10 SDK from `global.json`. The core library and its unit tests target .NET 8, 9, and 10; the AI, console, system tests, and AI tests target .NET 10. Install the corresponding runtimes when running all tests.

```sh
dotnet build Skunked.sln -c Release
dotnet test Skunked.sln -c Release
dotnet run --project examples/GettingStarted/GettingStarted.csproj -c Release
pwsh ./scripts/Verify-Packages.ps1
```

GitHub Actions runs the solution build and tests, the core framework matrix, the getting-started example, and package verification on pull requests and pushes to `main` and `develop`. Benchmarks are run separately from `test/Skunked.Standard.Analytics`.

## Engine conventions

- `Cribbage` accepts two or four distinct player IDs and validates commands before emitting events. Four-player teams are positions 0 and 2 versus 1 and 3.
- `Cribbage.State` is a detached snapshot. Read it again after each command; `RoundState.Starter` is nullable until selection. Never change live game state outside the event builder.
- The engine appends ordered, game-specific versioned events. `GameStateBuilder.Replay` rebuilds state from an ordered history. Completion emits `GameCompletedEvent` and sets `CompletedAt`; a command after completion throws `GameFinishedException`.
- Opening-cut ties are recut by the tied players. After the cut, the dealer owns the crib; pegging and hand counting begin with the next player.
- Score the crib with `isCrib: true`; a four-card crib flush does not count. Declared scores below the calculated score are accepted, while negative scores and overcounts are rejected.
- External listener failures are recorded in `Cribbage.ListenerFailures`. Commands on a game are serialized; listeners must not issue reentrant commands.
- `GameRunner` drives `IGameRunnerPlayer` implementations. Its random factory and the `Cribbage` random/deck overload support reproducible simulations when player decisions are also deterministic.

Public API, XML documentation, framework support, examples, and package conventions are documented in `CONTRIBUTING.md`. Preserve the flat `Skunked` core namespace and keep public compatibility and release notes in mind when changing signatures or behavior.
