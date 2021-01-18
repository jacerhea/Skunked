using System;
using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.Events
{
    public class DeckShuffledEvent : StreamEvent
    {
        public DeckShuffledEvent(Guid gameId, int version, List<Card> deck) : base(gameId, version)
        {
            Deck = deck;
        }

        public List<Card> Deck { get; }
    }
}
