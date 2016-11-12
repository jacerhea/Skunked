﻿using System.Collections.Generic;
using Skunked.Rules;

namespace Skunked.State.Events
{
    public class GameStartedEvent : StreamEvent
    {
        public List<int> Players { get; set; }
        public GameRules Rules { get; set; }
    }
}