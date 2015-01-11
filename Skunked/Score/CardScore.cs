using System;

namespace Skunked.Score
{
    public class CardScore
    {
        public Card Card { get; private set; }
        public int Score { get; private set; }

        /// <summary>
        /// group a card and a possible score
        /// </summary>
        /// <param name="card"></param>
        /// <param name="score"></param>
        public CardScore(Card card, int score)
        {
            if (card == null) throw new ArgumentNullException("card");
            Card = card;
            Score = score;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Card, Score);
        }
    }
}
