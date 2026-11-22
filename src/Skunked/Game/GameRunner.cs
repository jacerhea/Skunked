namespace Skunked;

/// <summary>
/// Runs a game of cribbage with the given player strategies provided.
/// Used for testing, AI comparison, or any other need to run through many games automatically.
/// </summary>
public sealed class GameRunner
{
    private readonly Deck _deck;
    private readonly Func<Random> _randomFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameRunner"/> class.
    /// </summary>
    /// <param name="deck">A full deck copied for each game.</param>
    public GameRunner(Deck deck) : this(deck, () => Random.Shared) { }

    /// <summary>Creates a runner with a per-game random generator factory.</summary>
    /// <param name="deck">A full deck copied for each game.</param>
    /// <param name="randomFactory">Creates a random generator for each game; use separate seeded instances for reproducible concurrent games.</param>
    public GameRunner(Deck deck, Func<Random> randomFactory)
    {
        _deck = deck ?? throw new ArgumentNullException(nameof(deck));
        _randomFactory = randomFactory ?? throw new ArgumentNullException(nameof(randomFactory));
    }

    /// <summary>
    /// Run a game of cribbage using the provided <see cref="IGameRunnerPlayer"/>.
    /// </summary>
    /// <param name="players">The players.</param>
    /// <param name="winningScore">The winning score.</param>
    /// <returns>The game of cribbage after it has completed.</returns>
    public Cribbage Run(List<IGameRunnerPlayer> players, WinningScore winningScore)
    {
        ArgumentNullException.ThrowIfNull(players);
        if (players.Count is not (2 or 4) || players.Any(p => p is null) || players.Select(p => p.Id).Distinct().Count() != players.Count)
            throw new ArgumentException("A game requires two or four distinct players.", nameof(players));

        var gameRules = new GameRules(winningScore);

        var cribbage = new Cribbage(players.Select(p => p.Id), gameRules, random: _randomFactory(), deck: _deck);
        var gameState = cribbage.State;

        while (!gameState.OpeningRound.Complete)
        {
            var eligible = gameState.OpeningRound.EligiblePlayers.Count == 0
                ? gameState.PlayerIds.ToList() : gameState.OpeningRound.EligiblePlayers.ToList();
            foreach (var player in players.Where(p => eligible.Contains(p.Id)))
            {
                var cardsForCut = gameState.OpeningRound.Deck.ToList();
                cribbage.CutCard(new CutCardCommand(player.Id, player.CutCards(cardsForCut)));
            }
        }

        try
        {
            while (true)
            {
                var currentRound = gameState.GetCurrentRound();
                foreach (var playerDiscard in players)
                {
                    var tossed = playerDiscard.DetermineCardsToThrow(currentRound.DealtCards
                        .Single(p => p.PlayerId == playerDiscard.Id).Hand);
                    cribbage.ThrowCards(new ThrowCardsCommand(playerDiscard.Id, tossed));
                    if (gameState.IsGameFinished()) return cribbage;
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
                    var show = player.DetermineCardsToPlay(gameRules,
                        currentPlayerPlayItems.Select(playItem => playItem.Card).ToList(), handLeft);

                    cribbage.PlayCard(new PlayCardCommand(player.Id, show));
                    if (gameState.IsGameFinished()) return cribbage;
                }

                var startingPlayer = players.Single(player =>
                    player.Id == gameState.GetNextPlayerFrom(currentRound.PlayerCrib));
                foreach (var player in players.Infinite().Skip(players.IndexOf(startingPlayer)).Take(players.Count)
                             .ToList())
                {
                    var playerCount = player.CountHand(currentRound.Starter,
                        currentRound.Hands.Single(playerHand => playerHand.PlayerId == player.Id).Hand);
                    cribbage.CountHand(new CountHandCommand(player.Id, playerCount));
                    if (gameState.IsGameFinished()) return cribbage;
                }

                var cribCount = players.Single(p => p.Id == currentRound.PlayerCrib)
                    .CountCrib(currentRound.Starter, currentRound.Crib);

                cribbage.CountCrib(new CountCribCommand(currentRound.PlayerCrib, cribCount));
                    if (gameState.IsGameFinished()) return cribbage;
            }
        }
        catch (GameFinishedException)
        {
        }

        return cribbage;
    }
}