using Konsole;
using Skunked;
using Skunked.AI;
using Skunked.ConsoleApp;

var console = new Window();
var rules = new GameRules();
var human = new HumanConsolePlayer(1, console);
var ai = new AiPlayer.OptimizedPlayer(2);

console.WriteLine("Welcome to Cribbage!");
console.WriteLine("You are playing against the AI.");
console.WriteLine($"First to {rules.WinningScore} wins.");

var game = new GameRunner(new Deck()).Run([human, ai], WinningScore.Standard121);
var winningTeam = game.State.TeamScores.Single(team => team.Score >= rules.WinningScore);
console.WriteLine(winningTeam.Players.Contains(human.Id) ? "You win!" : "The AI wins!");
