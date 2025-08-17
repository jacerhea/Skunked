using Konsole;
using Skunked.AI;
using Skunked;
using System.Text;

namespace Skunked.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        var console = new Window();
        var rules = new GameRules();
        var human = new HumanConsolePlayer(1, console);
        var ai = new AiPlayer.OptimizedPlayer(2);
        var players = new List<IGameRunnerPlayer> { human, ai };

        console.WriteLine("Welcome to Cribbage!");
        console.WriteLine("You are playing against the AI.");
        console.WriteLine($"First to {rules.WinningScore} wins.");
        console.WriteLine("");

        var cribbage = new Cribbage([1, 2], rules, new List<IEventListener>());

        foreach (var player in players)
        {
            var cut = Draw(cribbage.State.OpeningRound.Deck);
            cribbage.CutCard(new CutCardCommand(player.Id, cut));
        }

        while (true)
        {
            var round = cribbage.State.GetCurrentRound();
            console.WriteLine("----------------------------------------");
            console.WriteLine(round.PlayerCrib == human.Id ? "You are the dealer." : "AI is the dealer.");

            var currentRound = cribbage.State.GetCurrentRound();
            foreach (var playerDiscard in players)
            {
                var tossed = playerDiscard.DetermineCardsToThrow(currentRound.DealtCards
                    .Single(p => p.PlayerId == playerDiscard.Id).Hand);
                cribbage.ThrowCards(new ThrowCardsCommand(playerDiscard.Id, tossed));
            }

            while (!currentRound.PlayedCardsComplete)
            {
                var currentPlayerPlayItems = currentRound.ThePlay.Last();
                var lastPlayerPlayItem = currentRound.ThePlay.SelectMany(ppi => ppi).LastOrDefault();
                var isFirstPlay = currentRound.ThePlay.Count == 1 && lastPlayerPlayItem == null;
                var player = isFirstPlay
                    ? players.NextOf(players.Single(p => p.Id == currentRound.PlayerCrib))
                    : players.Single(p => p.Id == lastPlayerPlayItem.NextPlayer);
                var playedCards = currentRound.ThePlay.SelectMany(ppi => ppi).Select(ppi => ppi.Card).ToList();
                var handLeft = currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand
                    .Except(playedCards).ToList();
                var show = player.DetermineCardsToPlay(cribbage.State.GameRules,
                    currentPlayerPlayItems.Select(playItem => playItem.Card).ToList(), handLeft);

                cribbage.PlayCard(new PlayCardCommand(player.Id, show));
            }

            var startingPlayer = players.Single(player =>
                player.Id == cribbage.State.GetNextPlayerFrom(currentRound.PlayerCrib));
            foreach (var player in players.Infinite().Skip(players.IndexOf(startingPlayer)).Take(players.Count)
                         .ToList())
            {
                var playerCount = player.CountHand(currentRound.Starter,
                    currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand);
                cribbage.CountHand(new CountHandCommand(player.Id, playerCount));
            }

            var cribCount = players.Single(p => p.Id == currentRound.PlayerCrib)
                .CountHand(currentRound.Starter, currentRound.Crib);

            cribbage.CountCrib(new CountCribCommand(currentRound.PlayerCrib, cribCount));
        }
    }

    private static Card Draw(List<Card> deck)
    {
        var c = deck[0];
        deck.RemoveAt(0);
        return c;
    }
}
