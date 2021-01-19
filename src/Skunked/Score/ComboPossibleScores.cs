using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Skunked.Cards;

namespace Skunked.Score
{
    /// <summary>
    /// Couple a set of cards with all of the possible scoring outcomes for that combo.
    /// </summary>
    public class ComboPossibleScores
    {
        public List<Card> Combo { get; }

        public List<ScoreWithCut> PossibleScores { get; }

        public ComboPossibleScores(IEnumerable<Card> combo, IEnumerable<ScoreWithCut> possibleScores)
        {
            if (combo == null) throw new ArgumentNullException(nameof(combo));
            Combo = combo.ToList();
            PossibleScores = possibleScores.ToList();
        }

        public int GetScoreSummation()
        {
            return PossibleScores.Sum(s => s.Score);
        }

        public override string ToString()
        {
            var psString = string.Join(", ", PossibleScores.Select(s => s.Score.ToString(CultureInfo.InvariantCulture)).ToArray());
            var cString = string.Join(", ", Combo.Select(c => c.ToString()).ToArray());

            return $"{psString} : {{{cString}}}";
        }
    }

    public class ScoreWithCut
    {
        public int Score { get; set; }

        public Card Cut { get; set; }
    }
}