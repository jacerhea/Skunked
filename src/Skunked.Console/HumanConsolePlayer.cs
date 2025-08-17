using Konsole;

namespace Skunked.ConsoleApp;

public sealed class HumanConsolePlayer : IGameRunnerPlayer
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
        var list = hand
            .OrderBy(card => card.Rank)
            .ThenBy(card => card.Suit)
            .ToList();
        _console.WriteLine("");
        _console.WriteLine("Choose two cards to throw to the crib.");
        foreach (var (index, card) in list.Index())
        {
            _console.WriteLine($"[{index}] {DescribeCard(card)}");
        }

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
        var list = cardsToChoose
            .OrderBy(card => card.Rank)
            .ThenBy(card => card.Suit)
            .ToList();
        foreach (var (index, card) in list.Index())
        {
            _console.WriteLine($"[{index}] {DescribeCard(card)}");
        }

        _console.WriteLine("Select a card to cut by index.");
        var idx = ReadIndex(0, list.Count - 1);
        return list[idx];
    }

    public int CountHand(Card starter, IEnumerable<Card> hand)
    {
        var list = hand.ToList();
        _console.WriteLine("");
        _console.WriteLine($"Count with starter: {DescribeCard(starter)}");
        foreach (var (index, card) in list.Index())
        {
            _console.WriteLine($"[{index}] {DescribeCard(card)}");
        }

        _console.WriteLine("Enter the total points you count for this hand:");
        return ReadInt(0, 29);
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