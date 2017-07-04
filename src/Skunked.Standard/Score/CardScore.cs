using System;
using Skunked.PlayingCards;

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
            Card = card ?? throw new ArgumentNullException(nameof(card));
            Score = score;
        }

        public override string ToString()
        {
            return $"{Card} : {Score}";
        }
    }
}
