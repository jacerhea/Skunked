﻿using System.Collections.Generic;
using Skunked.PlayingCards;

namespace Skunked.State.Events
{
    public class DeckShuffledEvent : Event
    {
        public List<Card> PostShuffleDeck { get; set; }
    }
}