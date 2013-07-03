using System;
using System.Collections.Generic;
using System.Linq;
using Games.Domain.MainModule.Entities.PlayingCards;

namespace Games.Domain.MainModule.Entities.CardGames.Cribbage.Score
{
    public class ComboScore
    {
        public List<ICard> Combo { get; private set; }
        public int Score { get; private set; }

        public ComboScore(List<ICard> combo, int score)
        {
            if (combo == null) throw new ArgumentNullException("combo");
            Combo = combo;
            Score = score;
        }

        public override string ToString()
        {
            return string.Format("{0} : {{{1}}}", Score, string.Join(", ", (Combo.Select(c => c.ToString()).ToArray())));
        }
    }
}