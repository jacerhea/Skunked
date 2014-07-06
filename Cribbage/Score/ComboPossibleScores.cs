using System;
using System.Collections.Generic;
using System.Linq;

namespace Skunked.Score
{
    /// <summary>
    /// Couple a set of cards with all of the possible scoring outcomes for that combo
    /// </summary>
    public class ComboPossibleScores
    {
        public IList<Card> Combo { get; private set; }
        public IList<int> PossibleScores { get; private set; }

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
            var psString = string.Join(", ", (PossibleScores.Select(s => s.ToString())).ToArray());
            var cString = string.Join(", ", (Combo.Select(c => c.ToString()).ToArray()));

            return string.Format("{0} : {{{1}}}", psString, cString);
        }
    }
}