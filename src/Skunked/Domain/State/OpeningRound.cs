﻿using System.Collections.Generic;
using Skunked.Cards;

namespace Skunked.Domain.State;

public class OpeningRound
{
    public List<Card> Deck { get; set; } = [];

    public List<PlayerIdCard> CutCards { get; set; }

    public bool Complete { get; set; }

    public int? WinningPlayerCut { get; set; }
}