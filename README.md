
Advanced Cribbage library for .NET

## Example Usage

Here's a basic example of how to start and run a cribbage game:

```
// new game of cribbage with 2 players
var player = 1;
var player2 = 2;
var players = new List<int> { player, player2 };
var state = new GameState();
var rules = new GameRules(WinningScoreType.Standard121);
var cribbage = new Cribbage(players, rules);
        
// Send commands to the game.  Here is the opening round where both players cut a card to see who goes first.
var gameState = cribbage.State; 
cribbage.CutCard(new CutCardCommand(player, gameState.OpeningRound.Deck[17]));
cribbage.CutCard(new CutCardCommand(player2, gameState.OpeningRound.Deck[46]));

```
## Supported frameworks

The core library targets .NET 8, .NET 9, and .NET 10. Its unit tests run against every target in CI. The AI, console, web, and system-test projects target .NET 10. When changing core framework support, update the target list, CI matrix, and this section together.

## Packaging

Build packages with `dotnet pack src/Skunked/Skunked.csproj -c Release` and `dotnet pack src/Skunked.AI/Skunked.AI.fsproj -c Release`. Run `pwsh ./scripts/Verify-Packages.ps1` to inspect their contents and build a consumer against the generated core package.
