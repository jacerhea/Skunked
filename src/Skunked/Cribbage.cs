using Skunked.Cards;
using Skunked.Domain;
using Skunked.Domain.Commands;
using Skunked.Domain.Events;
using Skunked.Domain.State;
using Skunked.Domain.Validations;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked;

/// <summary>
/// The class for creating and modeling a game of Cribbage. "Plays" are validated and stored as a stream of events. Event listeners allow de-normalized state to be created from the stream.
/// </summary>
public class Cribbage
{
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
        State = new GameState();
        var internalStateBuilder = new GameStateBuilder(State);
        var listeners = new List<IEventListener> { internalStateBuilder };
        listeners.AddRange(eventListeners ?? Enumerable.Empty<IEventListener>());
        Stream = new EventStream(listeners);

        Emit(new GameStartedEvent(Guid.NewGuid(), NewVersion, players.ToList(), rules));
        Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
    }


    /// <summary>
    /// Gets the state of the game.
    /// </summary>
    public GameState State { get; }

    /// <summary>
    /// Gets set of events that have occurred in the game.
    /// </summary>
    public EventStream Stream { get; }

    private int NewVersion => State.Version + 1;

    /// <summary>
    /// Cut the deck.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void CutCard(CutCardCommand command)
    {
        var validation = new CutCardCommandValidation();
        validation.Validate(State, command);

        var cardCutEvent = new CardCutEvent(State.Id, 1, command.PlayerId, command.CutCard);
        Emit(cardCutEvent);

        if (State.OpeningRound.Complete)
        {
            Emit(new RoundStartedEvent(State.Id, NewVersion));
            _deck.Shuffle();
            Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
            var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(command.PlayerId), State.GameRules.GetDealSize(State.PlayerIds.Count)); // get next player from who won cut.
            Emit(new HandsDealtEvent(State.Id, NewVersion, playerHands));
        }
    }

    /// <summary>
    /// Throw cards to the crib.
    /// </summary>
    /// <param name="command">Command to throw cards.</param>
    public void ThrowCards(ThrowCardsCommand command)
    {
        var validation = new ThrowCardsCommandValidation();
        var cardsThrown = new CardsThrownEvent(State.Id, NewVersion, command.PlayerId, command.CribCards.ToList());
        validation.Validate(State, command);

        Emit(cardsThrown);
        var currentRound = State.GetCurrentRound();
        if (currentRound.ThrowCardsComplete)
        {
            var cardsNotDealt = _deck.Except(currentRound.Crib).Except(currentRound.Hands.SelectMany(s => s.Hand)).ToList();
            var randomIndex = RandomProvider.GetThreadRandom().Next(0, cardsNotDealt.Count - 1);
            var startingCard = cardsNotDealt[randomIndex];
            Emit(new StarterCardSelectedEvent(State.Id, NewVersion, startingCard));
            Emit(new PlayStartedEvent(State.Id, NewVersion, currentRound.Round));
        }
    }

    /// <summary>
    /// Play a card.
    /// </summary>
    /// <param name="command">Command to play card.</param>
    public void PlayCard(PlayCardCommand command)
    {
        var validation = new PlayCardCommandValidation();
        var @event = new CardPlayedEvent(State.Id, NewVersion, command.PlayerId, command.Card);
        validation.Validate(State, command);
        Emit(@event);

        if (State.GetCurrentRound().PlayedCardsComplete)
        {
            Emit(new PlayFinishedEvent(State.Id, NewVersion, State.GetCurrentRound().Round));
        }
    }

    /// <summary>
    /// Count a players hand.
    /// </summary>
    /// <param name="command">Command to count hand.</param>
    public void CountHand(CountHandCommand command)
    {
        var validation = new CountHandCommandValidation();
        var @event = new HandCountedEvent(State.Id, NewVersion, command.PlayerId, command.Score);
        validation.Validate(State, command);
        Emit(@event);
    }

    /// <summary>
    /// Count a players crib.
    /// </summary>
    /// <param name="command">The id of the player.</param>
    public void CountCrib(CountCribCommand command)
    {
        var validation = new CountCribCommandValidation();
        var @event = new CribCountedEvent(State.Id, NewVersion, command.PlayerId, command.Score);
        validation.Validate(State, command);
        Emit(@event);

        Emit(new RoundStartedEvent(State.Id, NewVersion));
        _deck.Shuffle(3);
        Emit(new DeckShuffledEvent(State.Id, NewVersion, _deck.ToList()));
        var playerHands = _dealer.Deal(_deck, State.PlayerIds, State.PlayerIds.NextOf(State.PlayerIds.NextOf(command.PlayerId)), State.GameRules.GetDealSize(State.PlayerIds.Count));
        Emit(new HandsDealtEvent(State.Id, NewVersion, playerHands));
    }

    private void Emit(StreamEvent @event)
    {
        Stream.Add(@event);
    }
}