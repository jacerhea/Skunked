﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Skunked.Utility;

namespace Skunked.Cards;

/// <summary>
/// Deck of playing cards.
/// </summary>
public sealed class Deck : IEnumerable<Card>
{
    private static readonly List<Card> InitialDeck = Enum.GetValues<Rank>()
        .Cartesian(Enum.GetValues<Suit>())
        .Select(pair => new Card(pair.Item1, pair.Item2))
        .ToList();

    private readonly List<Card> _deck;

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class with 52 cards.
    /// </summary>
    public Deck()
    {
        _deck = InitialDeck.ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class with 52 cards.
    /// </summary>
    /// <param name="deck">Set of cards that make up the deck.</param>
    public Deck(IEnumerable<Card> deck)
    {
        _deck = deck.ToList();
    }

    /// <summary>
    /// Randomly shuffles the deck.
    /// </summary>
    public void Shuffle()
    {
        _deck.Shuffle();
    }


    /// <summary>
    /// Randomly shuffles the deck the given number of times.
    /// </summary>
    /// <param name="count">Number of times to shuffle the card.</param>
    public void Shuffle(int count)
    {
        foreach (var i in Enumerable.Range(1, count))
        {
            _deck.Shuffle();
        }
    }

    /// <inheritdoc/>
    public IEnumerator<Card> GetEnumerator()
    {
        return _deck.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}