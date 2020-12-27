using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.Cards;

namespace Skunked.Score
{
    public class ComboScore
    {
        public List<Card> Combo { get; }
        public int Score { get; }

        public ComboScore(List<Card> combo, int score)
        {
            Combo = combo ?? throw new ArgumentNullException(nameof(combo));
            Score = score;
        }

        public override string ToString()
        {
            return $"{Score} : {{{string.Join(", ", Combo.Select(c => c.ToString()).ToArray())}}}";
        }
    }
}