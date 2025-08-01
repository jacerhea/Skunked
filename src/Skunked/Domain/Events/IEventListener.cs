﻿namespace Skunked.Domain.Events;

/// <summary>
/// An event listener.
/// </summary>
public interface IEventListener
{
    /// <summary>
    /// Notify when an event occurs.
    /// </summary>
    /// <param name="event">The occurred event.</param>
    void Notify(GameEvent @event);
}