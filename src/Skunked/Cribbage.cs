namespace Skunked;

/// <summary>
/// The class for creating and modeling a game of Cribbage. "Plays" are validated and stored as a stream of events. Event listeners allow de-normalized state to be created from the stream.
/// </summary>
public sealed class Cribbage
{
    private readonly GameState _state = new();
    private readonly Dealer _dealer = new();
    private readonly Deck _deck = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Cribbage"/> class.
    /// </summary>
    /// <param name="players">Players in order by index and the value of the player id.</param>
    /// <param name="rules">The set of rules for the game.</param>
    /// <param name="eventListeners">Listen to all game events.</param>
    public Cribbage(IEnumerable<int> players, GameRules rules, IEnumerable<IEventListener>? eventListeners = null)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(rules);
        var playerIds = players.ToList();
        if (playerIds.Count is not (2 or 4) || playerIds.Distinct().Count() != playerIds.Count)
            throw new ArgumentException("A game requires two or four distinct players.", nameof(players));

        var internalStateBuilder = new GameStateBuilder(_state);
        var listeners = new List<IEventListener> { internalStateBuilder };
        listeners.AddRange(eventListeners ?? []);
        Stream = new EventStream(listeners);

        Emit(new GameStartedEvent(Guid.NewGuid(), NewVersion, playerIds, rules));
        _deck.Shuffle();
        Emit(new DeckShuffledEvent(_state.Id, NewVersion, _deck.ToList()));
    }


    /// <summary>
    /// Gets a detached snapshot of the game state. Request another snapshot after issuing a command.
    /// </summary>
    public GameState State => GameStateSnapshot.Copy(_state);

    internal GameState LiveState => _state;

    /// <summary>
    /// Gets set of events that have occurred in the game.
    /// </summary>
    public EventStream Stream { get; }

    private int NewVersion => _state.Version + 1;

    /// <summary>
    /// Cut the deck.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void CutCard(CutCardCommand command)
    {
        var validation = new CutCardCommandValidation();
        validation.Validate(_state, command);

        var cardCutEvent = new CardCutEvent(_state.Id, NewVersion, command.PlayerId, command.CutCard);
        Emit(cardCutEvent);

        if (_state.OpeningRound.Complete)
        {
            Emit(new RoundStartedEvent(_state.Id, NewVersion));
            _deck.Shuffle();
            Emit(new DeckShuffledEvent(_state.Id, NewVersion, _deck.ToList()));
            var playerHands = _dealer.Deal(_deck, _state.PlayerIds, _state.PlayerIds.NextOf(_state.OpeningRound.WinningPlayerCut!.Value),
                _state.GameRules.GetDealSize(_state.PlayerIds.Count)); // get next player from who won cut.
            Emit(new HandsDealtEvent(_state.Id, NewVersion, playerHands));
        }
    }

    /// <summary>
    /// Throw cards to the crib.
    /// </summary>
    /// <param name="command">Command to throw cards.</param>
    public void ThrowCards(ThrowCardsCommand command)
    {
        var validation = new ThrowCardsCommandValidation();
        var cardsThrown = new CardsThrownEvent(_state.Id, NewVersion, command.PlayerId, command.CribCards.ToList());
        validation.Validate(_state, command);

        Emit(cardsThrown);
        var currentRound = _state.GetCurrentRound();
        if (currentRound.ThrowCardsComplete)
        {
            var cardsNotDealt = _deck.Except(currentRound.Crib).Except(currentRound.Hands.SelectMany(s => s.Hand))
                .ToList();
            var randomIndex = ChooseStarterIndex(RandomProvider.GetThreadRandom(), cardsNotDealt.Count);
            var startingCard = cardsNotDealt[randomIndex];
            Emit(new StarterCardSelectedEvent(_state.Id, NewVersion, startingCard));
            if (_state.IsGameFinished()) return;
            Emit(new PlayStartedEvent(_state.Id, NewVersion, currentRound.Round));
        }
    }

    /// <summary>
    /// Play a card.
    /// </summary>
    /// <param name="command">Command to play card.</param>
    public void PlayCard(PlayCardCommand command)
    {
        var validation = new PlayCardCommandValidation();
        var @event = new CardPlayedEvent(_state.Id, NewVersion, command.PlayerId, command.Card);
        validation.Validate(_state, command);
        Emit(@event);

        if (!_state.IsGameFinished() && _state.GetCurrentRound().PlayedCardsComplete)
        {
            Emit(new PlayFinishedEvent(_state.Id, NewVersion, _state.GetCurrentRound().Round));
        }
    }

    /// <summary>
    /// Count a players hand.
    /// </summary>
    /// <param name="command">Command to count hand.</param>
    public void CountHand(CountHandCommand command)
    {
        var validation = new CountHandCommandValidation();
        var @event = new HandCountedEvent(_state.Id, NewVersion, command.PlayerId, command.Score);
        validation.Validate(_state, command);
        Emit(@event);
    }

    /// <summary>
    /// Count a players crib.
    /// </summary>
    /// <param name="command">The id of the player.</param>
    public void CountCrib(CountCribCommand command)
    {
        var validation = new CountCribCommandValidation();
        var @event = new CribCountedEvent(_state.Id, NewVersion, command.PlayerId, command.Score);
        validation.Validate(_state, command);
        Emit(@event);

        if (_state.IsGameFinished()) return;
        Emit(new RoundStartedEvent(_state.Id, NewVersion));
        _deck.Shuffle(3);
        Emit(new DeckShuffledEvent(_state.Id, NewVersion, _deck.ToList()));
        var playerHands = _dealer.Deal(_deck, _state.PlayerIds,
            _state.PlayerIds.NextOf(_state.PlayerIds.NextOf(command.PlayerId)),
            _state.GameRules.GetDealSize(_state.PlayerIds.Count));
        Emit(new HandsDealtEvent(_state.Id, NewVersion, playerHands));
    }

    internal static int ChooseStarterIndex(Random random, int count) => random.Next(count);

    private void Emit(StreamEvent @event)
    {
        Stream.Add(@event);
        if ((@event is StarterCardSelectedEvent or CardPlayedEvent or HandCountedEvent or CribCountedEvent) &&
            _state.IsGameFinished() && _state.CompletedAt is null)
            Stream.Add(new GameCompletedEvent(_state.Id, NewVersion));
    }
}