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
        var human = 1;
        var ai = 2;
        var players = new List<IGameRunnerPlayer>
            { new HumanConsolePlayer(1, console), CreateOpponent(ai) };

        console.WriteLine("Welcome to Cribbage!");
        console.WriteLine("You are playing against the AI.");
        console.WriteLine("This sample plays hands/crib counting (no pegging). First to 121 wins.");
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
            console.WriteLine(round.PlayerCrib == human ? "You are the dealer." : "AI is the dealer.");

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
            //
            // var startingPlayer = players.Single(player =>
            //     player.Id == gameState.GetNextPlayerFrom(currentRound.PlayerCrib));
            // foreach (var player in players.Infinite().Skip(players.IndexOf(startingPlayer)).Take(players.Count)
            //              .ToList())
            // {
            //     var playerCount = player.CountHand(currentRound.Starter,
            //         currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand);
            //     cribbage.CountHand(new CountHandCommand(player.Id, playerCount));
            // }
            //
            // var cribCount = players.Single(p => p.Id == currentRound.PlayerCrib)
            //     .CountHand(currentRound.Starter, currentRound.Crib);

            // cribbage.CountCrib(new CountCribCommand(currentRound.PlayerCrib, cribCount));
        }
    }

    private static IGameRunnerPlayer CreateOpponent(int id)
    {
        return new AiPlayer.OptimizedPlayer(id);
    }

    // Basic “deck” helpers (52-card deck)
    private static List<Card> BuildDeck()
    {
        var deck = new List<Card>(52);
        foreach (var suit in Enum.GetValues<Suit>())
        {
            foreach (var rank in Enum.GetValues<Rank>())
            {
                deck.Add(new Card(rank, suit));
            }
        }

        return deck;
    }

    private static void Shuffle<T>(IList<T> list)
    {
        var rng = new Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static List<Card> Deal(List<Card> deck, int count)
    {
        var result = deck.Take(count).ToList();
        deck.RemoveRange(0, count);
        return result;
    }

    private static Card Draw(List<Card> deck)
    {
        var c = deck[0];
        deck.RemoveAt(0);
        return c;
    }

    private static void RemoveCardsFromHand(List<Card> hand, List<Card> toRemove)
    {
        foreach (var c in toRemove)
        {
            // Remove by matching rank/suit
            var idx = hand.FindIndex(h => h.Suit == c.Suit && h.Rank == c.Rank);
            if (idx >= 0) hand.RemoveAt(idx);
        }
    }

    private static bool CheckWinner(int scoreHuman, int scoreAI, int target, IConsole console)
    {
        if (scoreHuman >= target)
        {
            console.WriteLine($"You reached {scoreHuman}!");
            return true;
        }

        if (scoreAI >= target)
        {
            console.WriteLine($"AI reached {scoreAI}!");
            return true;
        }

        return false;
    }

    private static string DescribeCard(Card c)
    {
        // Prefer Card.ToString() if it’s meaningful
        var s = c.ToString();
        if (!string.IsNullOrWhiteSpace(s) && !s.Contains(c.GetType().Name, StringComparison.OrdinalIgnoreCase))
            return s;
        return $"{c.Rank} of {c.Suit}";
    }

    private static void PrintCards(IConsole console, IReadOnlyList<Card> cards)
    {
        if (cards.Count == 0)
        {
            console.WriteLine("(none)");
            return;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            console.WriteLine($"[{i}] {DescribeCard(cards[i])}");
        }
    }

    private static T InvokeSafe<T>(Func<T> call, Func<T> fallback)
    {
        try
        {
            return call();
        }
        catch
        {
            return fallback();
        }
    }

    private static List<Card> SimpleRandomSelect(List<Card> hand, int count)
    {
        var rng = new Random();
        return hand.OrderBy(_ => rng.Next()).Take(count).ToList();
    }
}

// Human player implementation driven by console prompts
file sealed class HumanConsolePlayer : IGameRunnerPlayer
{
    private readonly IConsole _console;

    public HumanConsolePlayer(int id, IConsole console)
    {
        Id = id;
        _console = console;
    }

    public int Id { get; }

    public List<Card> DetermineCardsToThrow(IEnumerable<Card> hand)
    {
        var list = hand.ToList();
        _console.WriteLine("");
        _console.WriteLine("Choose two cards to throw to the crib.");
        for (int i = 0; i < list.Count; i++)
            _console.WriteLine($"[{i}] {DescribeCard(list[i])}");

        var indices = ReadIndices(2, list.Count);
        var result = indices.Select(i => list[i]).ToList();

        _console.WriteLine("You threw to the crib:");
        foreach (var c in result) _console.WriteLine($"- {DescribeCard(c)}");

        return result;
    }

    public Card DetermineCardsToPlay(GameRules gameRules, List<Card> pile, List<Card> handLeft)
    {
        // Pegging not used in this simplified sample. If used, prompt similarly.
        // Return the first card as placeholder to satisfy interface if called.
        return handLeft.First();
    }

    public Card CutCards(IEnumerable<Card> cardsToChoose)
    {
        var list = cardsToChoose.ToList();
        for (int i = 0; i < list.Count; i++)
            _console.WriteLine($"[{i}] {DescribeCard(list[i])}");
        _console.WriteLine("Select a card to cut by index.");
        var idx = ReadIndex(0, list.Count - 1);
        return list[idx];
    }

    public int CountHand(Card starter, IEnumerable<Card> hand)
    {
        var list = hand.ToList();
        _console.WriteLine("");
        _console.WriteLine($"Count with starter: {DescribeCard(starter)}");
        for (int i = 0; i < list.Count; i++)
            _console.WriteLine($"[{i}] {DescribeCard(list[i])}");

        _console.WriteLine("Enter the total points you count for this hand:");
        return ReadInt(0, 29); // typical max
    }

    private int ReadIndex(int min, int max)
    {
        while (true)
        {
            _console.Write("> ");
            var s = Console.ReadLine();
            if (int.TryParse(s, out var value) && value >= min && value <= max)
                return value;
            _console.WriteLine($"Enter a number between {min} and {max}.");
        }
    }

    private int[] ReadIndices(int count, int upperExclusive)
    {
        while (true)
        {
            _console.Write("> ");
            var line = (Console.ReadLine() ?? "");
            var parts = line
                .Replace(",", " ")
                .Replace(";", " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == count &&
                parts.All(p => int.TryParse(p, out var x) && x >= 0 && x < upperExclusive))
            {
                var indices = parts.Select(int.Parse).Distinct().ToArray();
                if (indices.Length == count) return indices;
                _console.WriteLine("Indices must be distinct.");
                continue;
            }

            _console.WriteLine($"Please enter {count} valid indices separated by space.");
        }
    }

    private int ReadInt(int min, int max)
    {
        while (true)
        {
            _console.Write("> ");
            var s = Console.ReadLine();
            if (int.TryParse(s, out var value) && value >= min && value <= max)
                return value;
            _console.WriteLine($"Enter a number between {min} and {max}.");
        }
    }

    private static string DescribeCard(Card c)
        => $"{c.Rank} of {c.Suit}";
}