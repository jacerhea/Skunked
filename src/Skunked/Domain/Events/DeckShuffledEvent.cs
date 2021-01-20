using System;
using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    public class DeckShuffledEvent : StreamEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckShuffledEvent"/> class.
        /// </summary>
        /// <param name="gameId">Unique identifier of the game.</param>
        /// <param name="version">The version of the game.</param>
        /// <param name="deck"></param>
        public DeckShuffledEvent(Guid gameId, int version, List<Card> deck)
            : base(gameId, version)
        {
            Deck = deck;
        }

        public List<Card> Deck { get; }
    }
}
