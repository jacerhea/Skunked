using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public HashSet<Card> Combo { get; private set; }
        public ReadOnlyCollection<ScoreWithCut> PossibleScores { get; set; }

        public ComboPossibleScores(IList<Card> combo, List<ScoreWithCut> possibleScores)
        {
            if (combo == null) throw new ArgumentNullException(nameof(combo));
            Combo = new HashSet<Card>(combo);
            PossibleScores = possibleScores.AsReadOnly();
        }

        public int GetScoreSummation()
        {
            return PossibleScores.Sum(s => s.Score);
        }

        public override string ToString()
        {
            var psString = string.Join(", ", (PossibleScores.Select(s => s.Score.ToString(CultureInfo.InvariantCulture))).ToArray());
            var cString = string.Join(", ", (Combo.Select(c => c.ToString()).ToArray()));

            return $"{psString} : {{{cString}}}";
        }
    }

    public class ScoreWithCut
    {
        public int Score { get; set; }
        public Card Cut { get; set; }
    }
}