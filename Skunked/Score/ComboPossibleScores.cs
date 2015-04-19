﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Skunked.PlayingCards;

namespace Skunked.Score
{
    /// <summary>
    /// Couple a set of cards with all of the possible scoring outcomes for that combo
    /// </summary>
    public class ComboPossibleScores
    {
        public IList<Card> Combo { get; private set; }
        private IList<int> PossibleScores { get; set; }

        public ComboPossibleScores(IList<Card> combo)
        {
            if (combo == null) throw new ArgumentNullException("combo");
            Combo = combo;
            PossibleScores = new List<int>(52);
        }

        public void AddScore(int score)
        {
            PossibleScores.Add(score);
        }

        public int GetScoreSummation()
        {
            return PossibleScores.Sum(s => s);
        }

        public override string ToString()
        {
            var psString = string.Join(", ", (PossibleScores.Select(s => s.ToString(CultureInfo.InvariantCulture))).ToArray());
            var cString = string.Join(", ", (Combo.Select(c => c.ToString()).ToArray()));

            return string.Format("{0} : {{{1}}}", psString, cString);
        }
    }
}