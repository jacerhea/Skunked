# Contributing

## Public API and compatibility

- Keep public types in the core library's `Skunked` namespace. Treat public signatures, event types and payloads, package targets, and documented behavior as compatibility commitments.
- Classify a change before release: document breaking changes and migration steps, and use an appropriate major-version change under semantic versioning. Additive, compatible features and fixes should use minor and patch versions respectively.
- Add XML comments for new or changed public members, including parameters, return values, exceptions, and important phase or ownership rules. Update the README and runnable example when consumer-facing behavior changes.
- Do not mutate a consumer's `Cribbage.State` snapshot to change a game. Commands validate and emit events; state is rebuilt by `GameStateBuilder`. Do not expose internal mutable state or event payloads.
- Keep score calculation independent of game state, and cover both hand and crib rules when changing show scoring.

## Validation

Run these commands from the repository root with the .NET 10 SDK and .NET 8, 9, and 10 runtimes installed:

```sh
dotnet build Skunked.sln -c Release
dotnet test Skunked.sln -c Release
dotnet run --project examples/GettingStarted/GettingStarted.csproj -c Release
pwsh ./scripts/Verify-Packages.ps1
```

The core unit tests target all supported core frameworks. Full-game system tests and AI tests target .NET 10. CI runs these checks on pull requests and pushes to `main` and `develop`. For changes to package targets or metadata, inspect the packages and update the README, framework matrix, and package verification together.

Keep the README C# example synchronized with `examples/GettingStarted/Program.cs`, which CI runs. Core XML documentation is generated during Release packaging; the package verification script checks that each target's XML documentation and symbols are present.
