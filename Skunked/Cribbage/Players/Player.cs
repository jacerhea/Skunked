using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Skunked.AI.CardToss;
using Skunked.AI.Count;
using Skunked.AI.Play;
using Skunked.Rules;
using Skunked.Utility;

namespace Skunked.Players
{
    public class Player : ISerializable
    {
        private IPlayStrategy _playStrategy;
        private IDecisionStrategy _decisionStrategy;
        private IScoreCountStrategy _scoreCountStrategy;

        public Player()
        {
            var guid = Guid.NewGuid();
            Name = guid.ToString();
            Id = RandomProvider.GetThreadRandom().Next();            
        }

        public Player(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            Id = RandomProvider.GetThreadRandom().Next();
        }

        public Player(string name, int id)
        {
            if (name == null) throw new ArgumentNullException("name");
            Id = id;
            Name = name;
        }

        public Player(string name, IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy, IScoreCountStrategy scoreCountStrategy)
        {
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
            _scoreCountStrategy = scoreCountStrategy;
            if (name == null) throw new ArgumentNullException("name");
            Id = RandomProvider.GetThreadRandom().Next();
            Name = name;
        }

        public string Name { get; private set; }
        public int Id { get; private set; }

        public void SetStrategies(IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy, IScoreCountStrategy scoreCountStrategy)
        {
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
            _scoreCountStrategy = scoreCountStrategy;            
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Id = info.GetInt32("Id");
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
            if (gameRules == null) throw new ArgumentNullException("gameRules");
            if (pile == null) throw new ArgumentNullException("pile");
            if (handLeft == null) throw new ArgumentNullException("handLeft");
            if(handLeft.Count == 0) throw new ArgumentException("handLeft");

            return _playStrategy.DetermineCardToThrow(gameRules, pile, handLeft);
        }

        public Card ChooseCard(List<Card> cardsToChoose)
        {
            if (cardsToChoose == null) throw new ArgumentNullException("cardsToChoose");
            var randomIndex = RandomProvider.GetThreadRandom().Next(0 ,cardsToChoose.Count);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            return _scoreCountStrategy.GetCount(card, hand);
        }

        public bool Equals(Player other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.Id == Id;
        }
    }
}