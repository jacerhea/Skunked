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
