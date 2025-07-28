using Skunked.Players;
using Skunked.Utility;

namespace Skunked.Cards;

/// <summary>
/// Standard dealer.  One card per pass.
/// </summary>
public class Dealer
{
    // todo: make the startingWith into the dealer.  this saves a step for the caller to figure out who needs to be dealt to first.

    /// <summary>
    /// Deals cards singly to each player starting with the given startingWith player.
    /// </summary>
    /// <param name="deck">The set of cards to deal from.</param>
    /// <param name="players">Set of players being dealt to.</param>
    /// <param name="startingWith">The player to start the deal with.</param>
    /// <param name="handSize">Number of cards to deal to each player.</param>
    /// <returns>Set of player hands in order from dealer.</returns>
    public List<PlayerHand> Deal(Deck deck, IList<int> players, int startingWith, int handSize)
    {
        ArgumentNullException.ThrowIfNull(players);
        var startingIndex = players.IndexOf(startingWith);
        var playersOrdered = players.Infinite().Skip(startingIndex).Take(players.Count).ToList();
        return playersOrdered.Select(p => new PlayerHand(p, deck.Skip(playersOrdered.IndexOf(p)).TakeEvery(players.Count).Take(handSize).ToList())).ToList();
    }
}