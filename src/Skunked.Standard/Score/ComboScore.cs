using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.PlayingCards;

namespace Skunked.Score
{
    public class ComboScore
    {
        public List<Card> Combo { get; private set; }
        public int Score { get; private set; }

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