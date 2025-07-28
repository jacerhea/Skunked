
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