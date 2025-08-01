﻿using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;

namespace Skunked.Score;

/// <summary>
/// Couple a set of cards with all of the possible scoring outcomes for that combo.
/// </summary>
public class ComboPossibleScores
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboPossibleScores"/> class.
    /// </summary>
    /// <param name="combo">The combination of cards.</param>
    /// <param name="possibleScores">Possible scores.</param>
    public ComboPossibleScores(IEnumerable<Card> combo, IEnumerable<ScoreWithCut> possibleScores)
    {
        ArgumentNullException.ThrowIfNull(combo);
        Combo = combo.ToList();
        PossibleScores = possibleScores.ToList();
    }

    /// <summary>
    /// The given combination of cards.
    /// </summary>
    public List<Card> Combo { get; }

    /// <summary>
    /// Possible scores given the combination and the starter.
    /// </summary>
    public List<ScoreWithCut> PossibleScores { get; }
}