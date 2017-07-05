﻿using System;

namespace Skunked.State.Events
{
    //should add an Guid?
    public abstract class StreamEvent
    {
        public Guid GameId { get; set; }
        public int Sequence { get; set; }
        public DateTimeOffset Occurred { get; set; } = DateTimeOffset.Now;
    }
}