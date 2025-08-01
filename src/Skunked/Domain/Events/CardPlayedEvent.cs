﻿using System;
using Skunked.Cards;

namespace Skunked.Domain.Events;

/// <summary>
/// Event when a card has been played.
/// </summary>
public class CardPlayedEvent : GameEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CardPlayedEvent"/> class.
    /// </summary>
    /// <param name="gameId">Unique identifier of the game.</param>
    /// <param name="version">The version of the game.</param>
    /// <param name="playerId">The id of the player.</param>
    /// <param name="played">The card played.</param>
    public CardPlayedEvent(Guid gameId, int version, int playerId, Card played)
        : base(gameId, version)
    {
        PlayerId = playerId;
        Played = played;
    }

    /// <summary>
    /// Gets the player id.
    /// </summary>
    public int PlayerId { get; }

    /// <summary>
    /// Gets the Card being played.
    /// </summary>
    public Card Played { get; }
}