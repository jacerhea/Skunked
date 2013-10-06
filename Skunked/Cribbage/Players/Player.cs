using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Cribbage.AI.CardToss;
using Cribbage.AI.TheCount;
using Cribbage.AI.ThePlay;
using Cribbage.Rules;

namespace Cribbage.Players
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
            ID = new Random().Next();            
        }

        public Player(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            Name = name;
            ID = new Random().Next();
        }

        public Player(string name, int id)
        {
            if (name == null) throw new ArgumentNullException("name");
            ID = id;
            Name = name;
        }

        public Player(string name, IPlayStrategy playStrategy, IDecisionStrategy decisionStrategy, IScoreCountStrategy scoreCountStrategy)
        {
            _playStrategy = playStrategy;
            _decisionStrategy = decisionStrategy;
            _scoreCountStrategy = scoreCountStrategy;
            if (name == null) throw new ArgumentNullException("name");
            ID = new Random().Next();
            Name = name;
        }

        public string Name { get; private set; }
        public int ID { get; private set; }

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
            return ID.GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            ID = info.GetInt32("ID");
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

        public Card PlayShow(CribGameRules gameRules, List<Card> pile, List<Card> handLeft)
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
            var randomGenerator = new Random();
            var randomIndex = randomGenerator.Next(0 ,cardsToChoose.Count);
            return cardsToChoose[randomIndex];
        }

        public int CountHand(Card card, IEnumerable<Card> hand)
        {
            return _scoreCountStrategy.GetCount(card, hand);
        }

        public bool Equals(Player other)
        {
            if (other == null) throw new ArgumentNullException("other");
            return other.ID == ID;
        }
    }
}