using System;
using System.Collections.Generic;
using System.Linq;
using Skunked.AI.CardToss;
using Skunked.AI.Play;
using Skunked.AI.Show;
using Skunked.PlayingCards;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Players
{
    public class Player : IEquatable<Player>
    {
        private readonly IPlayStrategy _playStrategy;
        private readonly IDecisionStrategy _decisionStrategy;
        private readonly IScoreCountStrategy _scoreCountStrategy;

        public Player(string name = null, int id = -1, IPlayStrategy playStrategy = null, IDecisionStrategy decisionStrategy = null, IScoreCountStrategy scoreCountStrategy = null)
        {
            var guid = Guid.NewGuid();
            Name = name ?? guid.ToString();
            Id = id == -1 ? RandomProvider.GetThreadRandom().Next() : id;
            _playStrategy = playStrategy ?? new MaxPlayStrategy();
            _decisionStrategy = decisionStrategy ?? new MaxAverageDecision();
            _scoreCountStrategy = scoreCountStrategy ?? new PercentageScoreCountStrategy();
        }

        public string Name { get; private set; }
        public int Id { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Deal Hand and return cards that will go back in crib
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Set of Cards to throw in crib.</returns>
        public List<Card> DealHand(IList<Card> hand)
        {
            return _decisionStrategy.DetermineCardsToThrow(hand).ToList();
        }

        public Card PlayShow(GameRules gameRules, List<Card> pile, List<Card> handLeft)
        {
            if (gameRules == null) throw new ArgumentNullException(nameof(gameRules));
            if (pile == null) throw new ArgumentNullException(nameof(pile));
            if (handLeft == null) throw new ArgumentNullException(nameof(handLeft));
            if(handLeft.Count == 0) throw new ArgumentException("handLeft");

            return _playStrategy.DetermineCardToThrow(gameRules, pile, handLeft);
        }

        public Card ChooseCard(List<Card> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException(nameof(cardsToChoose));
            var randomIndex = RandomProvider.GetThreadRandom().Next(0 ,cardsToChoose.Count - 1);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            return _scoreCountStrategy.GetCount(card, hand);
        }

        public bool Equals(Player other)
        {
            return other?.Id == Id;
        }
    }
}