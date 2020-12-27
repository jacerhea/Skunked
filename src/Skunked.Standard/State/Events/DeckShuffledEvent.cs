﻿using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.State.Events
{
    public class DeckShuffledEvent : StreamEvent
    {
        public List<Card> Deck { get; set; } = new();
    }
}
