namespace Skunked;

/// <summary>
/// The class for creating and modeling a game of Cribbage. "Plays" are validated and stored as a stream of events. Event listeners allow de-normalized state to be created from the stream.
/// </summary>
public sealed class Cribbage
{
    private readonly object _sync = new();
    private readonly Queue<StreamEvent> _notifications = new();
    private bool _dispatching;
    private readonly List<IEventListener> _listeners;
    private readonly System.Collections.Concurrent.ConcurrentQueue<Exception> _listenerFailures = new();
    private readonly GameState _state = new();
    private readonly Dealer _dealer = new();
    private readonly Deck _deck;
    private readonly Random _random;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cribbage"/> class.
    /// </summary>
    /// <param name="players">Players in order by index and the value of the player id.</param>
    /// <param name="rules">The set of rules for the game.</param>
    /// <param name="eventListeners">Listeners are notified in event order outside game locks. When another notification is in progress, commands can return before their notifications are delivered.</param>
    public Cribbage(IEnumerable<int> players, GameRules rules, IEnumerable<IEventListener>? eventListeners = null)
        : this(players, rules, Random.Shared, null, eventListeners)
    {
    }

    /// <summary>Creates a game using a supplied random generator and optional deck.</summary>
    /// <param name="players">Players in turn order.</param>
    /// <param name="rules">Rules for the game.</param>
    /// <param name="random">Per-game random generator for reproducible games.</param>
    /// <param name="deck">Optional full deck used for the game.</param>
    /// <param name="eventListeners">Listeners are notified in event order outside game locks. When another notification is in progress, commands can return before their notifications are delivered.</param>
    public Cribbage(IEnumerable<int> players, GameRules rules, Random random, Deck? deck, IEnumerable<IEventListener>? eventListeners = null)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(rules);
        var playerIds = players.ToList();
        if (playerIds.Count is not (2 or 4) || playerIds.Distinct().Count() != playerIds.Count)
            throw new ArgumentException("A game requires two or four distinct players.", nameof(players));

        _random = random ?? throw new ArgumentNullException(nameof(random));
        var cards = (deck ?? new Deck()).ToList();
        if (cards.Count != 52 || cards.Distinct().Count() != 52 || cards.Except(new Deck()).Any())
            throw new ArgumentException("The deck must contain the 52 standard cards.", nameof(deck));
        _deck = new Deck(cards);
        var internalStateBuilder = new GameStateBuilder(_state);
        _listeners = (eventListeners ?? []).Select(listener => new SafeListener(listener, _listenerFailures)).Cast<IEventListener>().ToList();
        Stream = new EventStream([internalStateBuilder]);

        Emit(new GameStartedEvent(Guid.NewGuid(), NewVersion, playerIds, rules));
        _deck.Shuffle(_random);
        Emit(new DeckShuffledEvent(_state.Id, NewVersion, _deck.ToList()));
        DispatchNotifications();
    }


    /// <summary>
    /// Gets a detached snapshot of the game state. Request another snapshot after issuing a command.
    /// </summary>
    public GameState State { get { lock (_sync) return GameStateSnapshot.Copy(_state); } }

    internal GameState LiveState => _state;

    /// <summary>
    /// Gets set of events that have occurred in the game.
    /// </summary>
    public EventStream Stream { get; }

    /// <summary>Exceptions raised by external listeners, in notification order.</summary>
    public IReadOnlyCollection<Exception> ListenerFailures => _listenerFailures.ToArray();

    private int NewVersion => _state.Version + 1;

    // A single drainer preserves event and listener order. Commands invoked by a listener
    // enqueue their events and return without waiting for the active drainer.
    private void Execute(Action action)
    {
        try { lock (_sync) action(); }
        finally { DispatchNotifications(); }
    }

    private void DispatchNotifications()
    {
        lock (_sync)
        {
            if (_dispatching || _notifications.Count == 0) return;
            _dispatching = true;
        }
        while (true)
        {
            StreamEvent item;
            lock (_sync)
            {
                if (_notifications.Count == 0)
                {
                    _dispatching = false;
                    return;
                }
                item = _notifications.Dequeue();
            }
            foreach (var listener in _listeners) listener.Notify(EventStream.CopyEvent(item));
        }
    }
    private sealed class SafeListener(IEventListener listener, System.Collections.Concurrent.ConcurrentQueue<Exception> failures) : IEventListener
    {
        public void Notify(StreamEvent item)
        {
            try { listener.Notify(item); }
            catch (Exception error) { failures.Enqueue(error); }
        }
    }

    /// <summary>
    /// Cut the deck.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void CutCard(CutCardCommand command) => Execute(() => CutCardCore(command));

    private void CutCardCore(CutCardCommand command)
    {
        var validation = new CutCardCommandValidation();
        validation.Validate(_state, command);

        var cardCutEvent = new CardCutEvent(_state.Id, NewVersion, command.PlayerId, command.CutCard);
        Emit(cardCutEvent);

        if (_state.OpeningRound.Complete)
        {
            Emit(new RoundStartedEvent(_state.Id, NewVersion));
            _deck.Shuffle(_random);
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
    public void ThrowCards(ThrowCardsCommand command) => Execute(() => ThrowCardsCore(command));

    private void ThrowCardsCore(ThrowCardsCommand command)
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
            var randomIndex = ChooseStarterIndex(_random, cardsNotDealt.Count);
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
    public void PlayCard(PlayCardCommand command) => Execute(() => PlayCardCore(command));

    private void PlayCardCore(PlayCardCommand command)
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
    public void CountHand(CountHandCommand command) => Execute(() => CountHandCore(command));

    private void CountHandCore(CountHandCommand command)
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
    public void CountCrib(CountCribCommand command) => Execute(() => CountCribCore(command));

    private void CountCribCore(CountCribCommand command)
    {
        var validation = new CountCribCommandValidation();
        var @event = new CribCountedEvent(_state.Id, NewVersion, command.PlayerId, command.Score);
        validation.Validate(_state, command);
        Emit(@event);

        if (_state.IsGameFinished()) return;
        Emit(new RoundStartedEvent(_state.Id, NewVersion));
        _deck.Shuffle(3, _random);
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
        _notifications.Enqueue(EventStream.CopyEvent(@event));
        if ((@event is StarterCardSelectedEvent or CardPlayedEvent or HandCountedEvent or CribCountedEvent) &&
            _state.IsGameFinished() && _state.CompletedAt is null)
        {
            var completion = new GameCompletedEvent(_state.Id, NewVersion);
            Stream.Add(completion);
            _notifications.Enqueue(completion);
        }
    }
}